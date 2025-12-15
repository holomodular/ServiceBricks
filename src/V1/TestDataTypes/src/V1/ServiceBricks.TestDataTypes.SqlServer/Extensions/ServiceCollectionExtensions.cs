using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.TestDataTypes.EntityFrameworkCore;
using ServiceBricks.Storage.SqlServer;

namespace ServiceBricks.TestDataTypes.SqlServer
{
    /// <summary>
    /// Extensions to add the TestDataTypesSqlServer module to the IServiceCollection.
    /// </summary>
    public static partial class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add the TestDataTypesSqlServer module to the IServiceCollection.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddServiceBricksTestDataTypesSqlServer(this IServiceCollection services, IConfiguration configuration)
        {
            // AI: Add the parent module
            services.AddServiceBricksTestDataTypesEntityFrameworkCore(configuration);

            // AI: Add the module to the ModuleRegistry
            ModuleRegistry.Instance.Register(TestDataTypesSqlServerModule.Instance);

            // AI: Add module business rules
            TestDataTypesSqlServerModuleAddRule.Register(BusinessRuleRegistry.Instance);
            ModuleSetStartedRule<TestDataTypesSqlServerModule>.Register(BusinessRuleRegistry.Instance);
            SqlServerDatabaseMigrationRule<TestDataTypesModule, TestDataTypesSqlServerContext>.Register(BusinessRuleRegistry.Instance);

            return services;
        }
    }
}
