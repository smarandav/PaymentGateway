namespace PaymentGateway.Application.Commands
{
    public class PaymentCommand
    {
        public PaymentCommand(string merchantId, string cardNumber, string nameOnCard, int cardExpiryMonth, int cardExpiryYear, decimal amount, string currency, string cvv)
        {
            MerchantId = merchantId;
            CardNumber = cardNumber;
            NameOnCard = nameOnCard;
            CardExpiryMonth = cardExpiryMonth;
            CardExpiryYear = cardExpiryYear;
            Amount = amount;
            Currency = currency;
            Cvv = cvv;
        }

        public string MerchantId { get; }
        public string CardNumber { get; }
        public string NameOnCard { get; }
        public int CardExpiryMonth { get; }
        public int CardExpiryYear { get; }
        public decimal Amount { get; }
        public string Currency { get; }
        public string Cvv { get; }
    }
}