using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using PaymentGateway.Domain.Repositories;
using PaymentGateway.Interfaces.BarclaysClient;
using PaymentGatewayApi;

namespace PaymentGateway.Tests.ApiInMemoryTests
{
    public class ApiApplicationFactory : WebApplicationFactory<BaseStartup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder
                .UseStartup<BaseStartup>()
                .ConfigureTestServices(services =>
                {
                    services.AddTransient<IMerchantRepository, InMemoryMerchantRepository>();
                    services.AddTransient<IPaymentRepository, InMemoryPaymentRepository>();
                    services.AddTransient<IBarclaysBankClient, InMemoryBarclaysBankClient>();
                })
                .ConfigureServices(x => x.AddLogging());
        }

        protected override IWebHostBuilder CreateWebHostBuilder()
        {
            return new WebHostBuilder();
        }
    }
}
