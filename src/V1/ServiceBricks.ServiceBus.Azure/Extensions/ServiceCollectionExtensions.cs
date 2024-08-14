using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ServiceBricks.ServiceBus.Azure
{
    /// <summary>
    /// Extension methods to add the ServiceBricks ServiceBus Azure module.
    /// </summary>
    public static partial class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add the ServiceBricks ServiceBus Azure module.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddServiceBricksServiceBusAzureQueue(this IServiceCollection services, IConfiguration configuration)
        {
            // AI: Add the module to the ModuleRegistry
            ModuleRegistry.Instance.RegisterItem(typeof(ServiceBusAzureModule), new ServiceBusAzureModule());

            // Remove the existing service bus
            var found = services.Where(x => x.ServiceType == typeof(IServiceBus)).FirstOrDefault();
            if (found != null)
                services.Remove(found);

            // Add the new service bus
            services.AddSingleton<IServiceBus, ServiceBusQueue>();
            services.AddSingleton<IServiceBusConnection, ServiceBusConnection>();

            return services;
        }

        /// <summary>
        /// Add the ServiceBricks ServiceBus Azure module.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddServiceBricksServiceBusAzureTopic(this IServiceCollection services, IConfiguration configuration)
        {
            // AI: Add the module to the ModuleRegistry
            ModuleRegistry.Instance.RegisterItem(typeof(ServiceBusAzureModule), new ServiceBusAzureModule());

            // Remove the existing service bus
            var found = services.Where(x => x.ServiceType == typeof(IServiceBus)).FirstOrDefault();
            if (found != null)
                services.Remove(found);

            // Add the new service bus
            services.AddSingleton<IServiceBus, ServiceBusTopic>();
            services.AddSingleton<IServiceBusConnection, ServiceBusConnection>();

            return services;
        }
    }
}