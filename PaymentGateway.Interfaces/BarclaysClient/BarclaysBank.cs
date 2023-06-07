using System.Threading.Tasks;
using FluentResults;
using Microsoft.Extensions.Logging;
using PaymentGateway.Application.PaymentApis;
using PaymentGateway.Domain.Errors;

namespace PaymentGateway.Interfaces.BarclaysClient
{
    public class BarclaysBank : IAcquiringBank
    {
        private readonly IBarclaysBankClient _barclaysBankClient;
        private readonly ILogger<BarclaysBank> _logger;

        public BarclaysBank(ILogger<BarclaysBank> logger, IBarclaysBankClient barclaysBankClient)
        {
            _logger = logger;
            _barclaysBankClient = barclaysBankClient;
        }

        public async Task<Result<PaymentResponse>> RequestPayment(PaymentRequest paymentRequest)
        {
            var response = await _barclaysBankClient.RequestPayment(paymentRequest.ToBarclaysPaymentRequest());

            if (response.Error != null)
            {
                _logger.LogError("Payment failed", response.Error);
                return Result.Fail(new PaymentFailedError());
            }

            return Result.Ok(response.Content.ToBarclaysPaymentResponse());
        }
    }
}
