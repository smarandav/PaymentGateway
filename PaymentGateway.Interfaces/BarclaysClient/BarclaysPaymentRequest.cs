using PaymentGateway.Application.PaymentApis;
using PaymentGateway.Domain;

namespace PaymentGateway.Interfaces.BarclaysClient
{
    public class BarclaysPaymentRequest : PaymentRequest
    {
        public BarclaysPaymentRequest(Merchant merchant, Payment payment) : base(merchant, payment)
        {
        }
    }
}