using PaymentGateway.Application.Commands;
using PaymentGateway.Domain;
using PaymentGatewayApi.Contracts;

namespace PaymentGatewayApi
{
    public static class Converters
    {
        public static PaymentCommand ToPaymentCommand(this PaymentRequest paymentRequest, string merchantId)
        {
            return new PaymentCommand(merchantId, paymentRequest.CardNumber, 
                            paymentRequest.NameOnCard, paymentRequest.CardExpiryMonth,
                            paymentRequest.CardExpiryYear, paymentRequest.Amount, 
                            paymentRequest.Currency, paymentRequest.Cvv);
        }

        public static PaymentResponse ToPaymentResponse(this Payment payment, string failureReason = null)
        {
            return new PaymentResponse(payment.Card.Number.Substring(payment.Card.Number.Length-4, 4), 
                payment.Card.NameOnCard, payment.Card.ExpiryMonth,
                payment.Card.ExpiryYear, payment.Id,
                payment.Status.ToString(), payment.MerchantId, failureReason);
        }
    }
}