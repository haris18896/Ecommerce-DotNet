using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductApi.Application.Interfaces;
using ProductApi.Infrustructure.Repositories;
using SharedLibrary.DependencyInjection;

namespace ProductApi.Infrustructure.Data.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrustructureService(this IServiceCollection services, IConfiguration config)
        {
            // Add database conectivity
            // Add authentication Scheme
            SharedServiceContainer.AddSharedServices<ProductDbContext>(services, config, config["MySerilog:FileName"]!);

            // Create Dependency Injection (DI)
            services.AddScoped<IProduct, ProductRepo>();

            return services;
        }

        public static IApplicationBuilder UserInfrustucturePolicy(this IApplicationBuilder app)
        {
            // Register middle such as
            // 1. Global Exception: handles external errors.
            // 2. Listen to Only Api Gateway: Blocks all outsider calls;

            SharedServiceContainer.UseSharedPlicies(app);

            return app;
        }
    }
}