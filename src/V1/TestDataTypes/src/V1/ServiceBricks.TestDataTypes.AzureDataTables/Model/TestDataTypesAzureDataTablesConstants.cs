namespace ServiceBricks.TestDataTypes.AzureDataTables
{
    /// <summary>
    /// Constants for the TestDataTypesAzureDataTables module.
    /// </summary>
    public static partial class TestDataTypesAzureDataTablesConstants
    {
        /// <summary>
        /// AppSetting key for the connection string.
        /// </summary>
        public const string APPSETTING_CONNECTION_STRING = "ServiceBricks:TestDataTypes:Storage:AzureDataTables:ConnectionString";

        /// <summary>
        /// Table name prefix for the module.
        /// </summary>
        public const string TABLENAME_PREFIX = "TestDataTypes";

        /// <summary>
        /// Get a table name for the module.
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static string GetTableName(string tableName)
        {
            return TABLENAME_PREFIX + tableName;
        }
    }
}
