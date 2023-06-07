using System.Threading.Tasks;
using FluentResults;
using PaymentGateway.Domain;
using PaymentGateway.Domain.Errors;
using PaymentGateway.Domain.Repositories;

namespace PaymentGateway.Tests.ApiInMemoryTests
{
    public class InMemoryMerchantRepository : IMerchantRepository
    {
        public Task<Result<Merchant>> GetMerchant(string merchantId)
        {
            if (merchantId == GatewayApiInMemoryTests.ExistingMerchnatId)
            {
                return Task.FromResult(Result.Ok(new Merchant(merchantId, "123456766", "123467")));
            }

            Result result = Result.Fail(new MerchantNotFoundError());
            return Task.FromResult((Result<Merchant>)result);
        }
    }
}