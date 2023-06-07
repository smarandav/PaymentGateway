using FluentResults;

namespace PaymentGateway.Domain.Errors
{
    public class PaymentFailedError : Error
    {
        public PaymentFailedError() : base("Payment failed")
        {
        }
    }
}