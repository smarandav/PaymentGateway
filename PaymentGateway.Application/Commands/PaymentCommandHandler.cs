using System.Linq;
using System.Threading.Tasks;
using FluentResults;
using PaymentGateway.Application.PaymentApis;
using PaymentGateway.Domain;
using PaymentGateway.Domain.Repositories;

namespace PaymentGateway.Application.Commands
{
    public class PaymentCommandHandler : IPaymentCommandHandler
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IAcquiringBank _acquiringBank;
        private readonly IMerchantRepository _merchantRepository;
        private readonly IPaymentRepository _paymentRepository;

        public PaymentCommandHandler(IDateTimeProvider dateTimeProvider, IAcquiringBank acquiringBank, IMerchantRepository merchantRepository, IPaymentRepository paymentRepository)
        {
            _dateTimeProvider = dateTimeProvider;
            _acquiringBank = acquiringBank;
            _merchantRepository = merchantRepository;
            _paymentRepository = paymentRepository;
        }

        public async Task<Result<Payment>> Handle(PaymentCommand paymentCommand)
        {
            Result<Payment> paymentResult = new Result<Payment>().WithSuccess("Payment succeeded");

            var payment = new Payment(paymentCommand.MerchantId, paymentCommand.Amount, paymentCommand.Currency);

            var paymentCardResult = payment.AddPaymentCard(new Card(paymentCommand.CardNumber, paymentCommand.NameOnCard,
                    paymentCommand.CardExpiryMonth, paymentCommand.CardExpiryYear, paymentCommand.Cvv),
                _dateTimeProvider.Now());

            if (!paymentCardResult.IsSuccess)
            {
                return await FailPayment(paymentCardResult, payment);
            }

            var merchantResult = await _merchantRepository.GetMerchant(paymentCommand.MerchantId);
            if (!merchantResult.IsSuccess)
            {
                return await FailPayment(merchantResult.ToResult(), payment);
            }

            var paymentResponse = await _acquiringBank.RequestPayment(new PaymentRequest(merchantResult.Value, payment));
            if (!paymentResponse.IsSuccess || paymentResponse.Value.StatusCode != "Success")
            {
                return await FailPayment(paymentResponse.ToResult(), payment);
            }

            payment.CompletePayment();
            await _paymentRepository.Upsert(payment);
            return paymentResult.WithValue(payment);
        }

        private async Task<Result<Payment>> FailPayment(Result result, Payment payment)
        {
            payment.FailPayment();

            await _paymentRepository.Upsert(payment);

            var failPayment = new Result<Payment>();
            failPayment.WithValue(payment);
            failPayment.WithError(result.Errors.FirstOrDefault());
            return failPayment;
        }
    }
}
