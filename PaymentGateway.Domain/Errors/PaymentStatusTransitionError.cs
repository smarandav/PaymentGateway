using FluentResults;

namespace PaymentGateway.Domain.Errors
{
    public class PaymentStatusTransitionError : Error
    {
        public PaymentStatusTransitionError() : base("Payment status transition error")
        {
        }
    }
}