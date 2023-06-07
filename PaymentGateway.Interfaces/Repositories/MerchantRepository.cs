using System.Threading.Tasks;
using FluentResults;
using PaymentGateway.Domain;
using PaymentGateway.Domain.Errors;
using PaymentGateway.Domain.Repositories;

namespace PaymentGateway.Interfaces.Repositories
{
    public class MerchantRepository : CosmosRepository<Merchant>, IMerchantRepository
    {
        public MerchantRepository(IDataStoreConfiguration dataStoreConfiguration)
            : base(dataStoreConfiguration.ConnectionString, dataStoreConfiguration.DatabaseName, dataStoreConfiguration.MerchantCollectionName)
        {
        }

        protected override string PartitionKeyValue(Merchant item)
        {
            return item.Id;
        }

        protected override string PrimaryKeyValue(Merchant item)
        {
            return item.Id;

        }

        public async Task<Result<Merchant>> GetMerchant(string merchantId)
        {
            var merchant = await GetItem(merchantId, merchantId);
            if (merchant != null)
            {
                return Result.Ok(merchant);
            }

            return Result.Fail(new MerchantNotFoundError());
        }
    }
}