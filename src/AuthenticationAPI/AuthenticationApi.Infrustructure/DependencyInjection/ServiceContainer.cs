using AuthenticationApi.Domain.DTOs.Interfaces;
using AuthenticationApi.Infrastructure.Data;
using AuthenticationApi.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedLibrary.DependencyInjection;

namespace AuthenticationApi.Infrastructure.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrustructureService(this IServiceCollection services, IConfiguration config)
        {
            // Add database Connectivity
            // JWT Add Authentication Scheme
            SharedServiceContainer.AddSharedServices<AuthenticationDbContext>(services, config, config["MySerilog:FileName"]!);

            // create Dependncy Injection
            services.AddScoped<IUser, UserRepository>();

            return services;
        }

        public static IApplicationBuilder UserInfrustructurePolicy(this IApplicationBuilder app)
        {
            // Register middleware such as
            // Global Excemption: Handle External errors.
            // Listen to Api Gateway
            SharedServiceContainer.UseSharedPlicies(app);
            return app;
        }
    }
}