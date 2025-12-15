using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ServiceBricks.Storage.AzureDataTables;

namespace ServiceBricks.TestDataTypes.AzureDataTables
{
    /// <summary>
    /// This is the storage repository for the TestDataTypesAzureDataTables module.
    /// </summary>
    /// <typeparam name="TDomain"></typeparam>
    public partial class TestDataTypesStorageRepository<TDomain> : AzureDataTablesStorageRepository<TDomain>
        where TDomain : class, IAzureDataTablesDomainObject<TDomain>, new()
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="logFactory"></param>
        /// <param name="configuration"></param>
        public TestDataTypesStorageRepository(
            ILoggerFactory logFactory,
            IConfiguration configuration)
            : base(logFactory)
        {
            ConnectionString = configuration.GetAzureDataTablesConnectionString(TestDataTypesAzureDataTablesConstants.APPSETTING_CONNECTION_STRING);
            TableName = TestDataTypesAzureDataTablesConstants.GetTableName(typeof(TDomain).Name);
        }
    }
}
