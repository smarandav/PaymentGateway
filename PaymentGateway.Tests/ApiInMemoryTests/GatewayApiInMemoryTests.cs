using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using PaymentGatewayApi.Contracts;
using Xunit;

namespace PaymentGateway.Tests.ApiInMemoryTests
{
    public class GatewayApiInMemoryTests : IDisposable
    {
        public const string ExistingMerchnatId = "12345670";
        public const string NotFoundgMerchnatId = "222";
        public const string ExistingPaymentId = "234";
        public const string NotFoundPaymentId = "23";

        private readonly ApiApplicationFactory _factory;
        private readonly HttpClient _client;

        public GatewayApiInMemoryTests()
        {
            _factory = new ApiApplicationFactory();
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task MerchantPaymentEndpoint_WithValidPaymentRequest_Returns_200()
        {
            var response = await _client.PostAsync($"api/merchant/{ExistingMerchnatId}/payment",
                new StringContent(JsonConvert.SerializeObject(CreatePaymentRequest()), Encoding.UTF8,
                    "application/json"));

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task MerchantPaymentEndpoint_WithValidPaymentRequest_Returns_SuccessfullPaymentStatus()
        {
            var response = await _client.PostAsync($"api/merchant/{ExistingMerchnatId}/payment",
                new StringContent(JsonConvert.SerializeObject(CreatePaymentRequest()), Encoding.UTF8,
                    "application/json"));

            var stringContent = await response.Content.ReadAsStringAsync();
            var objectContent = JsonConvert.DeserializeObject<PaymentResponse>(stringContent);

            objectContent.Status.Should().Be("Success");
        }
        
        [Fact]
        public async Task MerchantPaymentEndpoint_WithInvalidCardNumberRequest_Returns_400()
        {
            var response = await _client.PostAsync($"api/merchant/{ExistingMerchnatId}/payment",
                new StringContent(JsonConvert.SerializeObject(CreatePaymentRequest("")), Encoding.UTF8,
                    "application/json"));

            var stringContent = await response.Content.ReadAsStringAsync();

            stringContent.Should().Contain("CardNumber must have a value");
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task MerchantPaymentEndpoint_WithExpiredCardRequest_Returns_400()
        {
            var response = await _client.PostAsync($"api/merchant/{ExistingMerchnatId}/payment",
                new StringContent(JsonConvert.SerializeObject(CreatePaymentRequest(null, DateTime.Now.Year - 1)), Encoding.UTF8,
                    "application/json"));

            var stringContent = await response.Content.ReadAsStringAsync();
            var objectContent = JsonConvert.DeserializeObject<PaymentResponse>(stringContent);

            objectContent.FailureReason.Should().Contain("Invalid card");
            objectContent.Status.Should().Contain("Failed");
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task MerchantPaymentEndpoint_ForUnknownMerchant_Returns_400()
        {
            var response = await _client.PostAsync($"api/merchant/{NotFoundgMerchnatId}/payment",
                new StringContent(JsonConvert.SerializeObject(CreatePaymentRequest()), Encoding.UTF8,
                    "application/json"));

            var stringContent = await response.Content.ReadAsStringAsync();
            var objectContent = JsonConvert.DeserializeObject<PaymentResponse>(stringContent);

            objectContent.FailureReason.Should().Contain("Merchant not found");
            objectContent.Status.Should().Contain("Failed");
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GetPaymentEndpoint_ForExistingPayment_Returns_200()
        {
            var response = await _client.GetAsync($"api/merchant/{ExistingMerchnatId}/payment/{ExistingPaymentId}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetPaymentEndpoint_ReturnsExistingPayment()
        {
            var response = await _client.GetAsync($"api/merchant/{ExistingMerchnatId}/payment/{ExistingPaymentId}");

            var stringContent = await response.Content.ReadAsStringAsync();
            var objectContent = JsonConvert.DeserializeObject<PaymentResponse>(stringContent);

            objectContent.MerchantId.Should().Be(ExistingMerchnatId);
            objectContent.Id.Should().Be(ExistingPaymentId);
        }

        [Fact]
        public async Task GetPaymentEndpoint_WhenPaymentNotFound_Returns_404()
        {
            var response = await _client.GetAsync($"api/merchant/{ExistingMerchnatId}/payment/{NotFoundPaymentId}");

            var stringContent = await response.Content.ReadAsStringAsync();
            stringContent.Should().Contain("Payment not found");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        public PaymentRequest CreatePaymentRequest(string cardNumber = null, int? cardExpiryYear = null)
        {
            return new PaymentRequest()
            {
                Amount = 12, CardExpiryMonth = DateTime.Now.Month, 
                CardExpiryYear = cardExpiryYear ?? DateTime.Now.Year,
                CardNumber = cardNumber ?? "4111111111111111", Currency = "GBP",
                Cvv = "123", NameOnCard = "L S Smith"
            };
        }

        public void Dispose()
        {
            _factory.Dispose();
        }
    }
}