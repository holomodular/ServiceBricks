using Microsoft.Extensions.Configuration;

namespace ServiceBricks.Storage.EntityFrameworkCore
{
    public static class IConfigurationExtensions
    {
        public static string GetGeneralConnectionString(this IConfiguration configuration, string configKey)
        {
            string val = configuration.GetValue<string>(configKey);
            if (string.IsNullOrEmpty(val))
                return configuration.GetGeneralConnectionString();
            return val;
        }

        public static string GetGeneralConnectionString(this IConfiguration configuration)
        {
            return configuration.GetValue<string>(StorageEntityFrameworkCoreConstants.APPSETTING_GENERAL_CONNECTION);
        }

        public static string GetSqlServerConnectionString(this IConfiguration configuration, string configKey)
        {
            string val = configuration.GetValue<string>(configKey);
            if (string.IsNullOrEmpty(val))
                return configuration.GetSqlServerConnectionString();
            return val;
        }

        public static string GetSqlServerConnectionString(this IConfiguration configuration)
        {
            string val = configuration.GetValue<string>(StorageEntityFrameworkCoreConstants.APPSETTING_SQLSERVER_CONNECTION);
            if (string.IsNullOrEmpty(val))
                return configuration.GetGeneralConnectionString();
            return val;
        }

        public static string GetSqliteConnectionString(this IConfiguration configuration, string configKey)
        {
            string val = configuration.GetValue<string>(configKey);
            if (string.IsNullOrEmpty(val))
                return configuration.GetSqliteConnectionString();
            return val;
        }

        public static string GetSqliteConnectionString(this IConfiguration configuration)
        {
            string val = configuration.GetValue<string>(StorageEntityFrameworkCoreConstants.APPSETTING_SQLITE_CONNECTION);
            if (string.IsNullOrEmpty(val))
                return configuration.GetGeneralConnectionString();
            return val;
        }

        public static string GetCosmosConnectionString(this IConfiguration configuration, string configKey)
        {
            string val = configuration.GetValue<string>(configKey);
            if (string.IsNullOrEmpty(val))
                return configuration.GetCosmosConnectionString();
            return val;
        }

        public static string GetCosmosConnectionString(this IConfiguration configuration)
        {
            string val = configuration.GetValue<string>(StorageEntityFrameworkCoreConstants.APPSETTING_COSMOS_CONNECTION);
            if (string.IsNullOrEmpty(val))
                return configuration.GetGeneralConnectionString();
            return val;
        }

        public static string GetCosmosDatabase(this IConfiguration configuration, string configKey)
        {
            string val = configuration.GetValue<string>(configKey);
            if (string.IsNullOrEmpty(val))
                return configuration.GetCosmosDatabase();
            return val;
        }

        public static string GetCosmosDatabase(this IConfiguration configuration)
        {
            var val = configuration.GetValue<string>(StorageEntityFrameworkCoreConstants.APPSETTING_COSMOS_DATABASE);
            if (string.IsNullOrEmpty(val))
                return StorageEntityFrameworkCoreConstants.DEFAULT_DATABASE_NAME;
            return val;
        }

        public static string GetPostgresConnectionString(this IConfiguration configuration, string configKey)
        {
            string val = configuration.GetValue<string>(configKey);
            if (string.IsNullOrEmpty(val))
                return configuration.GetPostgresConnectionString();
            return val;
        }

        public static string GetPostgresConnectionString(this IConfiguration configuration)
        {
            var val = configuration.GetValue<string>(StorageEntityFrameworkCoreConstants.APPSETTING_POSTGRES_CONNECTION);
            if (string.IsNullOrEmpty(val))
                return configuration.GetGeneralConnectionString();
            return val;
        }
    }
}