using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderApi.Application.interfaces;
using OrderApi.Infrustructure.Data;
using OrderApi.Infrustructure.Repositories;
using SharedLibrary.DependencyInjection;

namespace OrderApi.Infrustructure.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfructrutureService(this IServiceCollection services, IConfiguration config)
        {
            // Add Database conectivity
            // Add Authentication scheme
            SharedServiceContainer.AddSharedServices<OrderDbContext>(services, config, config["MySerilog:FileName"]!);

            // Create Dependency Injection
            services.AddScoped<IOrder, OrderRepository>();

            return services;
        }

        public static IApplicationBuilder UserInfrustructurePolicy(this IApplicationBuilder app)
        {
            // Register middleware such as:
            // 1. Global Exception -> handle external errors
            // 2. ListenToApiGateway Only -> Block all outsiders calls

            SharedServiceContainer.UseSharedPlicies(app);
            return app;
        }
    }
}