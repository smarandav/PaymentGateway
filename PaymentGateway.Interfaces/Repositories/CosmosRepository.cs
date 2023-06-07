using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

namespace PaymentGateway.Interfaces.Repositories
{
    public abstract class CosmosRepository<T>
    {
        protected Container Container { get; }

        protected CosmosClient Client { get; }

        protected abstract string PartitionKeyValue(T item);

        protected abstract string PrimaryKeyValue(T item);

        protected CosmosRepository(string connectionString, string databaseName, string collectionName)
        {
            Client = new CosmosClient(connectionString);
            Container = Client.GetDatabase(databaseName).GetContainer(collectionName);
        }

        public async Task<T> UpsertAsync(T item)
        {
            var partitionKey = new PartitionKey(PartitionKeyValue(item));

            var result = await Container.UpsertItemAsync(item, partitionKey);
            return result;
        }

        public async Task<T> GetItem(string id, string partitionId)
        {
            try
            {
                PartitionKey partitionKey = string.IsNullOrEmpty(partitionId) ? PartitionKey.None : new PartitionKey(partitionId);
                var item = await Container.ReadItemAsync<T>(id, partitionKey);
                return item.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                return default;
            }
        }
    }
}