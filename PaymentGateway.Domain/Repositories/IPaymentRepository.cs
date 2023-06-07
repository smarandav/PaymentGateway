using System.Threading.Tasks;
using FluentResults;

namespace PaymentGateway.Domain.Repositories
{
    public interface IPaymentRepository
    {
        Task<Result<Payment>> Upsert(Payment payment);

        Task<Result<Payment>> Get(string paymentId, string merchantId);
    }
}