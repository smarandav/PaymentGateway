using System;
using System.Net.Http;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PaymentGateway.Application;
using PaymentGateway.Application.Commands;
using PaymentGateway.Application.PaymentApis;
using PaymentGateway.Application.Queries;
using PaymentGateway.Domain.Repositories;
using PaymentGateway.Interfaces.BarclaysClient;
using PaymentGateway.Interfaces.DataStoreClient;
using PaymentGateway.Interfaces.Repositories;
using PaymentGatewayApi.Configuration;
using PaymentGatewayApi.Validators;
using Refit;
using PaymentRequest = PaymentGatewayApi.Contracts.PaymentRequest;

namespace PaymentGatewayApi
{
    public class BaseStartup
    {
        public IConfiguration Configuration { get; }

        public BaseStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddFluentValidation();

            services.AddTransient<IValidator<PaymentRequest>, PaymentRequestValidator>();

            services.AddTransient<IPaymentCommandHandler, PaymentCommandHandler>();
            services.AddTransient<IPaymentQueryHandler, PaymentQueryHandler>();
            services.AddTransient<IDateTimeProvider, DateTimeProvider>();
            services.AddTransient<IAcquiringBank, BarclaysBank>();

            services.AddTransient<IPaymentRepository, PaymentRepository>();
            services.AddTransient<IMerchantRepository, MerchantRepository>();
            services.AddTransient<IDataStoreManager, DataStoreManager>();

            var barclaysBankConfiguration = new BarclaysBankConfiguration();
            Configuration.Bind("BarclaysBank", barclaysBankConfiguration);

            services.AddRefitClient<IBarclaysBankClient>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(barclaysBankConfiguration.HostUrl));

            var dataStoreConfiguration = new DataStoreConfiguration();
            Configuration.Bind("DataStore", dataStoreConfiguration);

            services.AddSingleton<IDataStoreConfiguration>(dataStoreConfiguration);

            const string githubusercontentUri = "https://raw.githubusercontent.com/qualified/challenge-data/master/words_alpha.txt";
        
            services.AddTransient<HttpClient>();
            services.AddHttpClient<LibraryService>(c => c.BaseAddress = new System.Uri("https://raw.githubusercontent.com/qualified/challenge-data/master/words_alpha.txt"));
            
            services.AddSingleton<ILibraryCache>(provider =>
            {
                var uri = "";
                var libraryService = provider.GetService<ILibraryService>();
                var words = libraryService.Download(uri).Result;
                return new InMemoryLibraryCache(words);
            });

            services.AddLogging();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}