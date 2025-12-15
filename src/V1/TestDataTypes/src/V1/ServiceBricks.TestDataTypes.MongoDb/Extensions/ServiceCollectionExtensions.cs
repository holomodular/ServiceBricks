using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Storage.MongoDb;

namespace ServiceBricks.TestDataTypes.MongoDb
{
    /// <summary>
    /// Extensions to add the TestDataTypesMongoDb module to the IServiceCollection.
    /// </summary>
    public static partial class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add the TestDataTypesMongoDb module to the IServiceCollection.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddServiceBricksTestDataTypesMongoDb(this IServiceCollection services, IConfiguration configuration)
        {
            // AI: Add the parent module
            services.AddServiceBricksTestDataTypes(configuration);

            // AI: Add this module to the ModuleRegistry
            ModuleRegistry.Instance.Register(TestDataTypesMongoDbModule.Instance);

            // AI: Add module business rules
            TestDataTypesMongoDbModuleAddRule.Register(BusinessRuleRegistry.Instance);
            MongoDbGuidSerializerStandardRule.Register(BusinessRuleRegistry.Instance);
            ModuleSetStartedRule<TestDataTypesMongoDbModule>.Register(BusinessRuleRegistry.Instance);

            return services;
        }
    }
}
