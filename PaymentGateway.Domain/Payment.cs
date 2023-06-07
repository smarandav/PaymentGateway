using System;
using FluentResults;
using Newtonsoft.Json;
using PaymentGateway.Domain.Errors;

namespace PaymentGateway.Domain
{
    public class Payment
    {
        public Payment()
        {
            
        }

        public Payment(string merchantId, decimal amount, string currency)
        {
            Id = Guid.NewGuid().ToString();
            MerchantId = merchantId;
            Amount = amount;
            Currency = currency;
            Status = PaymentStatus.InProgress;
        }

        public Payment(string paymentId, string merchantId, decimal amount, 
                string currency, Card card, PaymentStatus paymentStatus = PaymentStatus.InProgress)
        {
            Id = paymentId;
            MerchantId = merchantId;
            Amount = amount;
            Currency = currency;
            Status = paymentStatus;
            Card = card;
        }

        [JsonProperty("id")]
        public string  Id { get; set;  }
        public string  MerchantId { get; set; }
        public Card Card { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public PaymentStatus Status { get; set; }

        public Result AddPaymentCard(Card card, DateTime currentDateTime)
        {
            Card = card;

            if (card.IsExpired(currentDateTime))
            {
                return Result.Fail(new ExpiredCardError());
            }

            return Result.Ok();
        }

        public Result CompletePayment()
        {
            if (Status != PaymentStatus.InProgress)
            {
                return Result.Fail(new PaymentStatusTransitionError());
            }

            Status = PaymentStatus.Success;

            return Result.Ok();
        }

        public Result FailPayment()
        {
            if (Status != PaymentStatus.InProgress)
            {
                return Result.Fail(new PaymentStatusTransitionError());
            }

            Status = PaymentStatus.Failed;

            return Result.Ok();
        }
    }
}
