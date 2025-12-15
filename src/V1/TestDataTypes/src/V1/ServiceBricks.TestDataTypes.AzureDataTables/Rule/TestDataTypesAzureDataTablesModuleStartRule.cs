using Azure.Data.Tables;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Storage.AzureDataTables;

namespace ServiceBricks.TestDataTypes.AzureDataTables
{
    /// <summary>
    /// This rule is executed when the TestDataTypesAzureDataTables module is added.
    /// </summary>
    public sealed class TestDataTypesAzureDataTablesModuleStartRule : BusinessRule
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public TestDataTypesAzureDataTablesModuleStartRule()
        {
            Priority = PRIORITY_HIGH;
        }

        /// <summary>
        /// Register the rule
        /// </summary>
        public static void Register(IBusinessRuleRegistry registry)
        {
            registry.Register(
                typeof(ModuleStartEvent<TestDataTypesModule>),
                typeof(TestDataTypesAzureDataTablesModuleStartRule));
        }

        /// <summary>
        /// UnRegister the rule.
        /// </summary>
        public static void UnRegister(IBusinessRuleRegistry registry)
        {
            registry.UnRegister(
                typeof(ModuleStartEvent<TestDataTypesModule>),
                typeof(TestDataTypesAzureDataTablesModuleStartRule));
        }

        /// <summary>
        /// Execute the business rule.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override IResponse ExecuteRule(IBusinessRuleContext context)
        {
            var response = new Response();

            // AI: Make sure the context object is the correct type
            if (context == null || context.Object == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "context"));
                return response;
            }
            var e = context.Object as ModuleStartEvent<TestDataTypesModule>;
            if (e == null || e.DomainObject == null || e.ApplicationBuilder == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "context"));
                return response;
            }

            // AI: Perform logic

            // AI: Get the connection string
            var configuration = e.ApplicationBuilder.ApplicationServices.GetRequiredService<IConfiguration>();
            var connectionString = configuration.GetAzureDataTablesConnectionString(
                TestDataTypesAzureDataTablesConstants.APPSETTING_CONNECTION_STRING);

            // AI: Create each table in the module
            TableClient tableClient = null;

            
            tableClient = new TableClient(
                connectionString,
                TestDataTypesAzureDataTablesConstants.GetTableName(nameof(Test)));
            tableClient.CreateIfNotExists();


            return response;
        }
    }
}
