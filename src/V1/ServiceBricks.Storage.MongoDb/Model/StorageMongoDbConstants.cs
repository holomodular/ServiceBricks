namespace ServiceBricks.Storage.MongoDb
{
    /// <summary>
    /// These are constants used by the MongoDb storage provider.
    /// </summary>
    public static partial class StorageMongoDbConstants
    {
        /// <summary>
        /// AppSetting key for the connection string.
        /// </summary>
        public const string APPSETTING_CONNECTION_STRING = "ServiceBricks:Storage:MongoDb:ConnectionString";

        /// <summary>
        /// AppSetting key for the database name.
        /// </summary>
        public const string APPSETTING_DATABASE_NAME = "ServiceBricks:Storage:MongoDb:Database";

        /// <summary>
        /// The default database name.
        /// </summary>
        public const string DEFAULT_DATABASE_NAME = "ServiceBricks";

        /// <summary>
        /// The delimiter used to separate keys in the storage key.
        /// </summary>
        public const string STORAGEKEY_DELIMITER = "|";
    }
}