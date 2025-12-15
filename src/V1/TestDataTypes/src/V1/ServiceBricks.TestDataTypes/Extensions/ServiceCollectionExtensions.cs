using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ServiceBricks.TestDataTypes
{
    /// <summary>
    /// Extensions for adding the Test All DataTypes Microservice.
    /// </summary>
    public static partial class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add the TestDataTypes module to the service collection.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddServiceBricksTestDataTypes(this IServiceCollection services, IConfiguration configuration)
        {
            // AI: Add the module to the ModuleRegistry
            ModuleRegistry.Instance.Register(TestDataTypesModule.Instance);

            // AI: Add module business rules
            TestDataTypesModuleAddRule.Register(BusinessRuleRegistry.Instance);
            ModuleSetStartedRule<TestDataTypesModule>.Register(BusinessRuleRegistry.Instance);

            return services;
        }
    }
}
