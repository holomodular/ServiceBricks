namespace ServiceBricks.Storage.EntityFrameworkCore
{
    /// <summary>
    /// These are constants used by the Entity Framework Core storage provider.
    /// </summary>
    public static partial class StorageEntityFrameworkCoreConstants
    {
        /// <summary>
        /// Application setting for the general connection string.
        /// </summary>
        public const string APPSETTING_GENERAL_CONNECTION = "ServiceBricks:Storage:General:ConnectionString";

        /// <summary>
        /// Application setting for the general connection string.
        /// </summary>
        public const string APPSETTING_GENERAL_DATABASE = "ServiceBricks:Storage:General:Database";

        /// <summary>
        /// Application setting for the SQL Server connection string.
        /// </summary>
        public const string APPSETTING_SQLSERVER_CONNECTION = "ServiceBricks:Storage:SqlServer:ConnectionString";

        /// <summary>
        /// Application setting for the SQLite connection string.
        /// </summary>
        public const string APPSETTING_SQLITE_CONNECTION = "ServiceBricks:Storage:Sqlite:ConnectionString";

        /// <summary>
        /// Application setting for the Cosmos connection string.
        /// </summary>
        public const string APPSETTING_COSMOS_CONNECTION = "ServiceBricks:Storage:Cosmos:ConnectionString";

        /// <summary>
        /// Application setting for the Cosmos database.
        /// </summary>
        public const string APPSETTING_COSMOS_DATABASE = "ServiceBricks:Storage:Cosmos:Database";

        /// <summary>
        /// Application setting for the Postgres connection string.
        /// </summary>
        public const string APPSETTING_POSTGRES_CONNECTION = "ServiceBricks:Storage:Postgres:ConnectionString";

        /// <summary>
        /// Application setting for the Postgres connection string.
        /// </summary>
        public const string DEFAULT_DATABASE_NAME = "ServiceBricks";

        /// <summary>
        /// The delimiter for the storage key.
        /// </summary>
        public const string STORAGEKEY_DELIMITER = "|";
    }
}