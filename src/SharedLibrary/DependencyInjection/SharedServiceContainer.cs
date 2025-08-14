using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SharedLibrary.Middleware;

namespace SharedLibrary.DependencyInjection
{
    public static class SharedServiceContainer
    {
        public static IServiceCollection AddSharedServices<TContext>(this IServiceCollection services, IConfiguration configuration, string fileName) where TContext : DbContext
        {
            // Add Generic Database Context
            services.AddDbContext<TContext>(option => option.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"), sqlserverOption => sqlserverOption.EnableRetryOnFailure()
            ));

            // configure Serilog logging
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Debug()
                .WriteTo.Console()
                .WriteTo.File(path: $"{fileName}-.text", restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information, outputTemplate: "{Timestamp:yyyy-MM=dd mm:ss.fff zzz} {level: u3} {message: lj}{Exception}", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            // Add JWT authentication Scheme
            JWTAuthenticationScheme.AddJwtAuthenticationScheme(services, configuration);

            return services;
        }

        public static IApplicationBuilder UseSharedPlicies(this IApplicationBuilder app)
        {
            // use Global Excemption
            app.UseMiddleware<GlobalException>();

            // Register middle ware to block all outsider API calls
            // app.UseMiddleware<ListenToOnlyApiGateway>(); TODO: Uncomment this line when API_GateWay is created

            return app;
        }
    }
}