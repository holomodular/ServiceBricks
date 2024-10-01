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
            ModuleRegistry.Instance.Register(ServiceBusAzureModule.Instance);

            // AI: Add module business rules
            ServiceBusAzureModuleQueueAddRule.Register(BusinessRuleRegistry.Instance);
            ServiceBusAzureModuleStartRule.Register(BusinessRuleRegistry.Instance);
            ModuleSetStartedRule<ServiceBusAzureModule>.Register(BusinessRuleRegistry.Instance);

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
            ModuleRegistry.Instance.Register(ServiceBusAzureModule.Instance);

            // AI: Add module business rules
            ServiceBusAzureModuleTopicAddRule.Register(BusinessRuleRegistry.Instance);
            ServiceBusAzureModuleStartRule.Register(BusinessRuleRegistry.Instance);
            ModuleSetStartedRule<ServiceBusAzureModule>.Register(BusinessRuleRegistry.Instance);

            return services;
        }
    }
}