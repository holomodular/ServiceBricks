namespace ServiceBricks.TestDataTypes.Cosmos
{
    /// <summary>
    /// Constants for the TestDataTypesCosmos module.
    /// </summary>
    public static partial class TestDataTypesCosmosConstants
    {
        /// <summary>
        /// Application setting for the connection string.
        /// </summary>
        public const string APPSETTING_CONNECTION_STRING = "ServiceBricks:TestDataTypes:Storage:Cosmos:ConnectionString";

        /// <summary>
        /// Application setting for the database name.
        /// </summary>
        public const string APPSETTING_DATABASE = "ServiceBricks:TestDataTypes:Storage:Cosmos:Database";

        /// <summary>
        /// The prefix for the collection name.
        /// </summary>
        public const string CONTAINER_PREFIX = "TestDataTypes";

        /// <summary>
        /// Get the collection name for the given table name.
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static string GetContainerName(string tableName)
        {
            return CONTAINER_PREFIX + tableName;
        }
    }
}
