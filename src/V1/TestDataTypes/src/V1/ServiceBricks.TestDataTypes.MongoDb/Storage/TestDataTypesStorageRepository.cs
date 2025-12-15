using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ServiceBricks.Storage.MongoDb;

namespace ServiceBricks.TestDataTypes.MongoDb
{
    /// <summary>
    /// This is the storage repository for the TestDataTypesMongoDb module.
    /// </summary>
    /// <typeparam name="TDomain"></typeparam>
    public partial class TestDataTypesStorageRepository<TDomain> : MongoDbStorageRepository<TDomain>
        where TDomain : class, IMongoDbDomainObject<TDomain>, new()
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
            ConnectionString = configuration.GetMongoDbConnectionString(TestDataTypesMongoDbConstants.APPSETTING_CONNECTION_STRING);
            DatabaseName = configuration.GetMongoDbDatabase(TestDataTypesMongoDbConstants.APPSETTING_DATABASE);
            CollectionName = TestDataTypesMongoDbConstants.GetCollectionName(typeof(TDomain).Name);
        }
    }
}
