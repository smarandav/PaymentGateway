namespace PaymentGatewayApi.Contracts
{
    public class PaymentRequest
    {
        public string CardNumber { get; set; }
        public string NameOnCard { get; set; }
        public int CardExpiryMonth { get; set; }
        public int CardExpiryYear { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string Cvv { get; set; }
    }
}