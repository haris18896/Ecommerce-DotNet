using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderApi.Application.Services;
using Polly;
using Polly.Retry;
using SharedLibrary.Logger;

namespace OrderApi.Application.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            // Register HttpClient Service
            // Create Dependency Injection
            services.AddHttpClient<IOrderService, OrderService>(options =>
            {
                options.BaseAddress = new Uri(config["ApiGateway:BaseAddress"]!);
                options.Timeout = TimeSpan.FromSeconds(1);
            });

            // Create Rerty Strategy
            var retryStrategy = new RetryStrategyOptions()
            {
                ShouldHandle = new PredicateBuilder().Handle<TaskCanceledException>(),
                BackoffType = DelayBackoffType.Constant,
                UseJitter = true,
                MaxRetryAttempts = 3,
                Delay = TimeSpan.FromMilliseconds(500),
                OnRetry = args =>
                {
                    string message = $"OnRetry, Attempt: {args.AttemptNumber} Outcome {args.Outcome}";
                    LogException.LogToConsole(message);
                    LogException.LogToDebugger(message);

                    return ValueTask.CompletedTask;
                }

            };

            // use Retry Strategy
            services.AddResiliencePipeline("my-retry-pipeline", builder =>
            {
                builder.AddRetry(retryStrategy);
            });

            return services;
        }
    }
}