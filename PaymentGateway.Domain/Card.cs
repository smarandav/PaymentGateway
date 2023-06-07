using System;

namespace PaymentGateway.Domain
{
    public class Card
    {
        public Card(string number, string nameOnCard, int expiryMonth, int expiryYear, string cvv)
        {
            Number = number;
            NameOnCard = nameOnCard;
            ExpiryMonth = expiryMonth;
            ExpiryYear = expiryYear;
            Cvv = cvv;
        }

        public string Number { get; set; }
        public string NameOnCard { get; set; }
        public int ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }
        public string Cvv { get; set; }

        public bool IsExpired(DateTime currentDateTime)
        {
            var expiryDate = new DateTime(ExpiryYear, ExpiryMonth, DateTime.DaysInMonth(ExpiryYear, ExpiryMonth));
            return currentDateTime > expiryDate;
        }
    }
}