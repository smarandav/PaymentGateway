using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using PaymentGateway.Interfaces.Repositories;

namespace PaymentGateway.Interfaces.DataStoreClient
{
    public class DataStoreManager : IDataStoreManager
    {
        private readonly IDataStoreConfiguration _dataStoreConfiguration;

        public DataStoreManager(IDataStoreConfiguration dataStoreConfiguration)
        {
            _dataStoreConfiguration = dataStoreConfiguration;
        }

        public async Task EnsureDataStoreIsCreated()
        {
            DatabaseResponse response = await new CosmosClient(_dataStoreConfiguration.ConnectionString)
                .CreateDatabaseIfNotExistsAsync(_dataStoreConfiguration.DatabaseName);

            var merchantCollectionTask = response.Database.CreateContainerIfNotExistsAsync(
                _dataStoreConfiguration.MerchantCollectionName,
                _dataStoreConfiguration.MerchantCollectionPartitionKeyName);

            var paymentCollectionTask = response.Database.CreateContainerIfNotExistsAsync(
                _dataStoreConfiguration.PaymentCollectionName,
                _dataStoreConfiguration.PaymentCollectionPartitionKeyName);

            Task.WaitAll(merchantCollectionTask, paymentCollectionTask);
        }
    }
}