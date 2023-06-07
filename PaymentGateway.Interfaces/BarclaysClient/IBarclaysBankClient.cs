using System.Threading.Tasks;
using Refit;

namespace PaymentGateway.Interfaces.BarclaysClient
{
    public interface IBarclaysBankClient
    {
        [Post("/payment")]
        public Task<ApiResponse<BarclaysPaymentResponse>> RequestPayment([Body] BarclaysPaymentRequest paymentRequest);
    }
}