namespace PaymentGatewayApi.Contracts
{
    public class PaymentResponse
    {
        public PaymentResponse(string cardNumber, string nameOnCard, int cardExpiryMonth, int cardExpiryYear, 
            string id, string status, string merchantId, string failureReason = null)
        {
            CardNumber = cardNumber;
            NameOnCard = nameOnCard;
            CardExpiryMonth = cardExpiryMonth;
            CardExpiryYear = cardExpiryYear;
            Id = id;
            Status = status;
            MerchantId = merchantId;
            FailureReason = failureReason;
        }

        public string CardNumber { get; }
        public string NameOnCard { get; }
        public int CardExpiryMonth { get; }
        public int CardExpiryYear { get; }
        public string Id { get; }
        public string Status { get; }
        public string FailureReason { get; }
        public string MerchantId { get; }
    }
}