using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ServiceBricks.ServiceBus.Azure
{
    /// <summary>
    /// IServiceCollection extensions for the Service Bus Brick.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServiceBricksServiceBusAzure(this IServiceCollection services, IConfiguration configuration)
        {
            // Add to module registry for reference
            ModuleRegistry.Instance.RegisterItem(typeof(ServiceBusAzureModule), new ServiceBusAzureModule());

            var found = services.Where(x => x.ServiceType == typeof(IServiceBus)).FirstOrDefault();
            if (found != null)
                services.Remove(found);
            services.AddSingleton<IServiceBus, ServiceBusQueue>();
            services.AddSingleton<IServiceBusConnection, ServiceBusConnection>();

            return services;
        }

        public static IServiceCollection AddServiceBricksServiceBusAzureAdvanced(this IServiceCollection services, IConfiguration configuration)
        {
            var found = services.Where(x => x.ServiceType == typeof(IServiceBus)).FirstOrDefault();
            if (found != null)
                services.Remove(found);
            services.AddSingleton<IServiceBus, ServiceBusTopic>();
            services.AddSingleton<IServiceBusConnection, ServiceBusConnection>();

            return services;
        }
    }
}