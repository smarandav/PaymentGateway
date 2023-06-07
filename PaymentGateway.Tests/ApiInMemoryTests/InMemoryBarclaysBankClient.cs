using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using PaymentGateway.Interfaces.BarclaysClient;
using Refit;

namespace PaymentGateway.Tests.ApiInMemoryTests
{
    public class InMemoryBarclaysBankClient : IBarclaysBankClient
    {
        public Task<ApiResponse<BarclaysPaymentResponse>> RequestPayment(BarclaysPaymentRequest paymentRequest)
        {
            if (paymentRequest.MerchantId == GatewayApiInMemoryTests.ExistingMerchnatId)
            {
                return Task.FromResult(new ApiResponse<BarclaysPaymentResponse>(
                    new HttpResponseMessage(HttpStatusCode.OK), new BarclaysPaymentResponse() { StatusCode = "Success" },
                    new RefitSettings(new NewtonsoftJsonContentSerializer())));
            }

            return Task.FromResult(new ApiResponse<BarclaysPaymentResponse>(
                new HttpResponseMessage(HttpStatusCode.InternalServerError), new BarclaysPaymentResponse(),
                new RefitSettings(new NewtonsoftJsonContentSerializer())));
        }
    }
}