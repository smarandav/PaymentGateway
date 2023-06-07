using PaymentGateway.Domain;

namespace PaymentGateway.Application.PaymentApis
{
    public class PaymentRequest
    {
        public PaymentRequest(Merchant merchant, Payment payment)
        {
            CardCvv = payment.Card.Cvv;
            MerchantId = merchant.Id;
            MerchantAccountNumber = merchant.AccountNumber;
            MerchantSortCode = merchant.SortCode;
            PaymentId = payment.Id;
            CardNumber = payment.Card.Number;
            NameOnCard = payment.Card.NameOnCard;
            CardExpiryMonth = payment.Card.ExpiryMonth;
            CardExpiryYear = payment.Card.ExpiryYear;
            Amount = payment.Amount;
            Currency = payment.Currency;
        }

        public string MerchantAccountNumber { get; }
        public string MerchantSortCode { get; }
        public string PaymentId { get; }
        public string MerchantId { get; }
        public string CardNumber { get; }
        public string CardCvv { get; }
        public string NameOnCard { get; }
        public int CardExpiryMonth { get; }
        public int CardExpiryYear { get; }
        public decimal Amount { get;  }
        public string Currency { get; }
    }
}