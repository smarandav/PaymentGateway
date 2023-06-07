using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PaymentGateway.Interfaces.Repositories;
using PaymentGatewayApi;
using PaymentGatewayApi.Configuration;

namespace PaymentGateway.Tests.ApiIntegrationTests
{
    public class ApiApplicationFactory : WebApplicationFactory<Startup>
    {
        public DataStoreConfiguration DataStoreConfigurationForTest { get; }

        public ApiApplicationFactory()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
                .AddJsonFile("appsettings.json", false)
                .Build();

            DataStoreConfigurationForTest = new DataStoreConfiguration();
            configuration.Bind("DataStore", DataStoreConfigurationForTest);
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder
                .UseStartup<Startup>()
                .ConfigureServices(x => x.AddLogging());
        }

        protected override IWebHostBuilder CreateWebHostBuilder()
        {
            return new WebHostBuilder();
        }
    }
}
