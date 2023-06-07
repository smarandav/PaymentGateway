namespace PaymentGateway.Interfaces.Repositories
{
    public interface IDataStoreConfiguration
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string MerchantCollectionName { get; set; }
        public string MerchantCollectionPartitionKeyName { get; set; }
        public string PaymentCollectionName { get; set; }
        public string PaymentCollectionPartitionKeyName { get; set; }
    }
}