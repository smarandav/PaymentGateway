using System.Threading.Tasks;
using FluentResults;

namespace PaymentGateway.Domain.Repositories
{
    public interface IMerchantRepository
    {
        Task<Result<Merchant>> GetMerchant(string merchantId);
    }
}