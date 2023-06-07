using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using MbDotNet;
using MbDotNet.Enums;
using MbDotNet.Models.Imposters;
using MbDotNet.Models.Predicates;
using MbDotNet.Models.Predicates.Fields;
using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;
using PaymentGateway.Domain;
using PaymentGatewayApi.Contracts;
using Xunit;

namespace PaymentGateway.Tests.ApiIntegrationTests
{
    public class GatewayApiIntegrationTests : IDisposable
    {
        public const string ExistingMerchnatId = "12345670";
        public const string ExistingPaymentId = "234";

        private readonly ApiApplicationFactory _factory;
        private readonly HttpClient _client;

        private readonly Container _merchantsContainer;
        private readonly Container _paymentsContainer;

        readonly MountebankClient _mountebankClient;
        readonly HttpImposter _barclaysApiHttpImposter;

        public GatewayApiIntegrationTests()
        {
            _factory = new ApiApplicationFactory();
            _client = _factory.CreateClient();

            var client = new CosmosClient(_factory.DataStoreConfigurationForTest.ConnectionString);

            _merchantsContainer = client.GetDatabase(_factory.DataStoreConfigurationForTest.DatabaseName)
                .GetContainer(_factory.DataStoreConfigurationForTest.MerchantCollectionName);

            _paymentsContainer = client.GetDatabase(_factory.DataStoreConfigurationForTest.DatabaseName)
                .GetContainer(_factory.DataStoreConfigurationForTest.PaymentCollectionName);

            _mountebankClient = new MountebankClient();
            _mountebankClient.DeleteAllImposters();

            _barclaysApiHttpImposter = _mountebankClient.CreateHttpImposter(4545, "BarclaysApi");
        }

        private void SetupBarclaysStubApiPaymentEndpoint()
        {
            var predicate = new MatchesPredicate<HttpPredicateFields>(
                new HttpPredicateFields()
                {
                    Path = "/payment",
                    Method = Method.Post
                });

            _barclaysApiHttpImposter.AddStub()
                .On(predicate)
                .ReturnsBody(HttpStatusCode.OK, "{ \"StatusCode\": \"Success\" }");

            _mountebankClient.Submit(_barclaysApiHttpImposter);
        }

        private async Task CreateTestMerchant()
        {
            await _merchantsContainer.UpsertItemAsync(new Merchant(ExistingMerchnatId, "33323232", "234567"), new PartitionKey(ExistingMerchnatId));
        }

        private async Task CreateTestPayment()
        {
           
            await _paymentsContainer.UpsertItemAsync(
                new Payment(ExistingPaymentId, ExistingMerchnatId, 123, "GBP", new Card("121212121", "L S Smith", 12, 2021, "345"), PaymentStatus.Success),
                new PartitionKey(ExistingMerchnatId));
        }

        [Fact]
        public async Task MerchantPaymentEndpoint_WithValidPaymentRequest_Returns_200()
        {
            SetupBarclaysStubApiPaymentEndpoint();
            await CreateTestMerchant();

            var response = await _client.PostAsync($"api/merchant/{ExistingMerchnatId}/payment",
                new StringContent(JsonConvert.SerializeObject(CreatePaymentRequest()), Encoding.UTF8,
                    "application/json"));

          
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var stringContent = await response.Content.ReadAsStringAsync();
            var objectContent = JsonConvert.DeserializeObject<PaymentResponse>(stringContent);

            objectContent.Status.Should().Be("Success");
        }

        [Fact]
        public async Task GetPaymentEndpoint_ForExistingPayment_Returns_200()
        {
            await CreateTestPayment();

            var response = await _client.GetAsync($"api/merchant/{ExistingMerchnatId}/payment/{ExistingPaymentId}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var stringContent = await response.Content.ReadAsStringAsync();
            var objectContent = JsonConvert.DeserializeObject<PaymentResponse>(stringContent);

            objectContent.MerchantId.Should().Be(ExistingMerchnatId);
            objectContent.Id.Should().Be(ExistingPaymentId);
            objectContent.Status.Should().Be("Success");
        }

        public PaymentRequest CreatePaymentRequest(string cardNumber = null, int? cardExpiryYear = null)
        {
            return new PaymentRequest()
            {
                Amount = 12,
                CardExpiryMonth = DateTime.Now.Month,
                CardExpiryYear = cardExpiryYear ?? DateTime.Now.Year,
                CardNumber = cardNumber ?? "4111111111111111",
                Currency = "GBP",
                Cvv = "123",
                NameOnCard = "L S Smith"
            };
        }

        public void Dispose()
        {
            _factory.Dispose();
            _client.Dispose();

            //todo add test date cleanup here 
        }
    }
}