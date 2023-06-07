using System.Threading.Tasks;
using FluentResults;
using PaymentGateway.Domain;
using PaymentGateway.Domain.Repositories;

namespace PaymentGateway.Application.Queries
{
    public class PaymentQueryHandler : IPaymentQueryHandler
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentQueryHandler(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<Result<Payment>> Get(string paymentId, string merchantId)
        {
            return await _paymentRepository.Get(paymentId, merchantId);
        }
    }
}