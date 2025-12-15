using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.TestDataTypes.EntityFrameworkCore;
using ServiceBricks.Storage.Sqlite;

namespace ServiceBricks.TestDataTypes.Sqlite
{
    /// <summary>
    /// Extensions to add the TestDataTypesSqlite module to the IServiceCollection.
    /// </summary>
    public static partial class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add the TestDataTypesSqlite module to the IServiceCollection.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddServiceBricksTestDataTypesSqlite(this IServiceCollection services, IConfiguration configuration)
        {
            // AI: Add the parent module
            services.AddServiceBricksTestDataTypesEntityFrameworkCore(configuration);

            // AI: Add the module to the ModuleRegistry
            ModuleRegistry.Instance.Register(TestDataTypesSqliteModule.Instance);

            // AI: Add module business rules
            TestDataTypesSqliteModuleAddRule.Register(BusinessRuleRegistry.Instance);
            ModuleSetStartedRule<TestDataTypesSqliteModule>.Register(BusinessRuleRegistry.Instance);
            SqliteDatabaseMigrationRule<TestDataTypesModule, TestDataTypesSqliteContext>.Register(BusinessRuleRegistry.Instance);

            return services;
        }
    }
}
