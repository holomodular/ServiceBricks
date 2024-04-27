namespace ServiceBricks.Storage.EntityFrameworkCore
{
    public static class StorageEntityFrameworkCoreConstants
    {
        public const string APPSETTING_GENERAL_CONNECTION = "ServiceBricks:Storage:General:ConnectionString";
        public const string APPSETTING_SQLSERVER_CONNECTION = "ServiceBricks:Storage:SqlServer:ConnectionString";
        public const string APPSETTING_SQLITE_CONNECTION = "ServiceBricks:Storage:Sqlite:ConnectionString";
        public const string APPSETTING_COSMOS_CONNECTION = "ServiceBricks:Storage:Cosmos:ConnectionString";
        public const string APPSETTING_COSMOS_DATABASE = "ServiceBricks:Storage:Cosmos:Database";
        public const string APPSETTING_POSTGRES_CONNECTION = "ServiceBricks:Storage:Postgres:ConnectionString";

        public const string DEFAULT_DATABASE_NAME = "ServiceBricks";

        public const string STORAGEKEY_DELIMITER = "|";
    }
}