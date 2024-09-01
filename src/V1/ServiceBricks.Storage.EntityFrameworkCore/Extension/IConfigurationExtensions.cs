using Microsoft.Extensions.Configuration;

namespace ServiceBricks.Storage.EntityFrameworkCore
{
    /// <summary>
    /// Configuration extensions.
    /// </summary>
    public static partial class IConfigurationExtensions
    {
        /// <summary>
        /// Get the general connection string.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="configKey"></param>
        /// <returns></returns>
        public static string GetGeneralConnectionString(this IConfiguration configuration, string configKey)
        {
            string val = configuration.GetValue<string>(configKey);
            if (string.IsNullOrEmpty(val))
                return configuration.GetGeneralConnectionString();
            return val;
        }

        /// <summary>
        /// Get the general connection string.
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static string GetGeneralConnectionString(this IConfiguration configuration)
        {
            return configuration.GetValue<string>(StorageEntityFrameworkCoreConstants.APPSETTING_GENERAL_CONNECTION);
        }

        /// <summary>
        /// Get the general connection string.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="configKey"></param>
        /// <returns></returns>
        public static string GetGeneralDatabase(this IConfiguration configuration, string configKey)
        {
            string val = configuration.GetValue<string>(configKey);
            if (string.IsNullOrEmpty(val))
                return configuration.GetGeneralDatabase();
            return val;
        }

        /// <summary>
        /// Get the general connection string.
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static string GetGeneralDatabase(this IConfiguration configuration)
        {
            string val = configuration.GetValue<string>(StorageEntityFrameworkCoreConstants.APPSETTING_GENERAL_DATABASE);
            if (string.IsNullOrEmpty(val))
                return StorageEntityFrameworkCoreConstants.DEFAULT_DATABASE_NAME;
            return val;
        }

        /// <summary>
        /// Get the SQL Server connection string.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="configKey"></param>
        /// <returns></returns>
        public static string GetSqlServerConnectionString(this IConfiguration configuration, string configKey)
        {
            string val = configuration.GetValue<string>(configKey);
            if (string.IsNullOrEmpty(val))
                return configuration.GetSqlServerConnectionString();
            return val;
        }

        /// <summary>
        /// Get the SQL Server connection string.
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static string GetSqlServerConnectionString(this IConfiguration configuration)
        {
            string val = configuration.GetValue<string>(StorageEntityFrameworkCoreConstants.APPSETTING_SQLSERVER_CONNECTION);
            if (string.IsNullOrEmpty(val))
                return configuration.GetGeneralConnectionString();
            return val;
        }

        /// <summary>
        /// Get the SQLite connection string.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="configKey"></param>
        /// <returns></returns>
        public static string GetSqliteConnectionString(this IConfiguration configuration, string configKey)
        {
            string val = configuration.GetValue<string>(configKey);
            if (string.IsNullOrEmpty(val))
                return configuration.GetSqliteConnectionString();
            return val;
        }

        /// <summary>
        /// Get the SQLite connection string.
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static string GetSqliteConnectionString(this IConfiguration configuration)
        {
            string val = configuration.GetValue<string>(StorageEntityFrameworkCoreConstants.APPSETTING_SQLITE_CONNECTION);
            if (string.IsNullOrEmpty(val))
                return configuration.GetGeneralConnectionString();
            return val;
        }

        /// <summary>
        /// Get the Cosmos connection string.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="configKey"></param>
        /// <returns></returns>
        public static string GetCosmosConnectionString(this IConfiguration configuration, string configKey)
        {
            string val = configuration.GetValue<string>(configKey);
            if (string.IsNullOrEmpty(val))
                return configuration.GetCosmosConnectionString();
            return val;
        }

        /// <summary>
        /// Get the Cosmos connection string.
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static string GetCosmosConnectionString(this IConfiguration configuration)
        {
            string val = configuration.GetValue<string>(StorageEntityFrameworkCoreConstants.APPSETTING_COSMOS_CONNECTION);
            if (string.IsNullOrEmpty(val))
                return configuration.GetGeneralConnectionString();
            return val;
        }

        /// <summary>
        /// Get the Cosmos database.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="configKey"></param>
        /// <returns></returns>
        public static string GetCosmosDatabase(this IConfiguration configuration, string configKey)
        {
            string val = configuration.GetValue<string>(configKey);
            if (string.IsNullOrEmpty(val))
                return configuration.GetCosmosDatabase();
            return val;
        }

        /// <summary>
        /// Get the Cosmos database.
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static string GetCosmosDatabase(this IConfiguration configuration)
        {
            var val = configuration.GetValue<string>(StorageEntityFrameworkCoreConstants.APPSETTING_COSMOS_DATABASE);
            if (string.IsNullOrEmpty(val))
                return configuration.GetGeneralDatabase();
            return val;
        }

        /// <summary>
        /// Get the Postgres connection string.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="configKey"></param>
        /// <returns></returns>
        public static string GetPostgresConnectionString(this IConfiguration configuration, string configKey)
        {
            string val = configuration.GetValue<string>(configKey);
            if (string.IsNullOrEmpty(val))
                return configuration.GetPostgresConnectionString();
            return val;
        }

        /// <summary>
        /// Get the Postgres connection string.
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static string GetPostgresConnectionString(this IConfiguration configuration)
        {
            var val = configuration.GetValue<string>(StorageEntityFrameworkCoreConstants.APPSETTING_POSTGRES_CONNECTION);
            if (string.IsNullOrEmpty(val))
                return configuration.GetGeneralConnectionString();
            return val;
        }
    }
}