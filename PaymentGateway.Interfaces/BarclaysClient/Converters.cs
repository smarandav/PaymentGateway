using PaymentGateway.Application.PaymentApis;
using PaymentGateway.Domain;

namespace PaymentGateway.Interfaces.BarclaysClient
{
    public static class Converters
    {
        public static BarclaysPaymentRequest ToBarclaysPaymentRequest(this PaymentRequest paymentRequest)
        {
            return new BarclaysPaymentRequest(
                new Merchant(paymentRequest.MerchantId, paymentRequest.MerchantAccountNumber,
                    paymentRequest.MerchantSortCode),
                new Payment(paymentRequest.PaymentId, paymentRequest.MerchantId, paymentRequest.Amount, paymentRequest.Currency,
                new Card(paymentRequest.CardNumber, paymentRequest.NameOnCard, paymentRequest.CardExpiryMonth,
                    paymentRequest.CardExpiryYear, paymentRequest.CardCvv)));
        }

        public static PaymentResponse ToBarclaysPaymentResponse(this BarclaysPaymentResponse paymentResponse)
        {
            return new BarclaysPaymentResponse()
                {StatusCode = paymentResponse.StatusCode, TransactionDateTime = paymentResponse.TransactionDateTime};
        }
    }
}