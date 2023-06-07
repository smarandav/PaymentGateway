using Newtonsoft.Json;

namespace PaymentGateway.Domain
{
    public class Merchant
    {
        public Merchant()
        {
            
        }

        public Merchant(string id, string accountNumber, string sortCode)
        {
            Id = id;
            AccountNumber = accountNumber;
            SortCode = sortCode;
        }

        [JsonProperty("id")]
        public string Id { get; set; }

        public string MerchantId => Id;

        public string AccountNumber { get; set; }
        public string SortCode { get; set; }
    }
}