using System.Threading.Tasks;
using FluentResults;
using PaymentGateway.Domain;

namespace PaymentGateway.Application.Queries
{
    public interface IPaymentQueryHandler
    {
        Task<Result<Payment>> Get(string paymentId, string merchantId);
    }
}