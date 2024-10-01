using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Storage.EntityFrameworkCore.Xunit.Rules;
using ServiceBricks.Xunit;

namespace ServiceBricks.Storage.EntityFrameworkCore.Xunit.Model
{
    /// <summary>
    /// IServiceCollection extensions for the Log module.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServiceBricksExampleInMemory(this IServiceCollection services, IConfiguration configuration)
        {
            // Add to module registry
            ModuleRegistry.Instance.Register(new ExampleModule());

            // Add module rules
            ExampleModuleAddRule.Register(BusinessRuleRegistry.Instance);

            return services;
        }

        public static IServiceCollection AddServiceBricksExampleClient(this IServiceCollection services, IConfiguration configuration)
        {
            // Clients
            services.AddScoped<IExampleApiClient, ExampleApiClient>();

            return services;
        }
    }
}