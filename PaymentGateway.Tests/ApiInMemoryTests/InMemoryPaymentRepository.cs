using System.Threading.Tasks;
using FluentResults;
using PaymentGateway.Domain;
using PaymentGateway.Domain.Errors;
using PaymentGateway.Domain.Repositories;

namespace PaymentGateway.Tests.ApiInMemoryTests
{
    public class InMemoryPaymentRepository : IPaymentRepository
    {
        public Task<Result<Payment>> Upsert(Payment payment)
        {
            return Task.FromResult(Result.Ok(payment));
        }

        public Task<Result<Payment>> Get(string paymentId, string merchantId)
        {
            if (paymentId == GatewayApiInMemoryTests.ExistingPaymentId)
            {
                return Task.FromResult(Result.Ok(
                    new Payment(paymentId, GatewayApiInMemoryTests.ExistingMerchnatId, 23, "GBP",
                        new Card("4111111111111111", "L S Smith", 2, 2022, "234"), PaymentStatus.Success)));
            }

            Result result = Result.Fail(new PaymentNotFoundError());
            return Task.FromResult((Result<Payment>)result);
        }
    }
}