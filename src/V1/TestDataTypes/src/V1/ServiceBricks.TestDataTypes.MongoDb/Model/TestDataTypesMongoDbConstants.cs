namespace ServiceBricks.TestDataTypes.MongoDb
{
    /// <summary>
    /// Constants for the TestDataTypesMongoDb module.
    /// </summary>
    public static partial class TestDataTypesMongoDbConstants
    {
        /// <summary>
        /// Application setting for the connection string.
        /// </summary>
        public const string APPSETTING_CONNECTION_STRING = "ServiceBricks:TestDataTypes:Storage:MongoDb:ConnectionString";

        /// <summary>
        /// Application setting for the database name.
        /// </summary>
        public const string APPSETTING_DATABASE = "ServiceBricks:TestDataTypes:Storage:MongoDb:Database";

        /// <summary>
        /// The prefix for the collection name.
        /// </summary>
        public const string COLLECTIONNAME_PREFIX = "TestDataTypes";

        /// <summary>
        /// Get the collection name for the given table name.
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static string GetCollectionName(string tableName)
        {
            return COLLECTIONNAME_PREFIX + tableName;
        }
    }
}
