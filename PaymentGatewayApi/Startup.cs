using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PaymentGateway.Interfaces.DataStoreClient;

namespace PaymentGatewayApi
{
    public class Startup: BaseStartup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public Startup(IConfiguration configuration) : base(configuration)
        {
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);

            var serviceProvider = services.BuildServiceProvider();

            // create database and collections if they dont exist
            serviceProvider.GetRequiredService<IDataStoreManager>()
                .EnsureDataStoreIsCreated()
                .GetAwaiter().GetResult();
        }
    }
}
