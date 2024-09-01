using Microsoft.Extensions.Configuration;

namespace ServiceBricks.Storage.MongoDb
{
    /// <summary>
    /// Extensions for IConfiguration.
    /// </summary>
    public static partial class IConfigurationExtensions
    {
        /// <summary>
        /// Get the MongoDB connection string.
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static string GetMongoDbConnectionString(this IConfiguration configuration)
        {
            return configuration.GetValue<string>(StorageMongoDbConstants.APPSETTING_CONNECTION_STRING);
        }

        /// <summary>
        /// Get the MongoDB connection string.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="configKey"></param>
        /// <returns></returns>
        public static string GetMongoDbConnectionString(this IConfiguration configuration, string configKey)
        {
            string val = configuration.GetValue<string>(configKey);
            if (string.IsNullOrEmpty(val))
                return configuration.GetValue<string>(StorageMongoDbConstants.APPSETTING_CONNECTION_STRING);
            return val;
        }

        /// <summary>
        /// Get the MongoDB database name.
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static string GetMongoDbDatabase(this IConfiguration configuration)
        {
            var val = configuration.GetValue<string>(StorageMongoDbConstants.APPSETTING_DATABASE_NAME);
            if (string.IsNullOrEmpty(val))
                return StorageMongoDbConstants.DEFAULT_DATABASE_NAME;
            return val;
        }

        /// <summary>
        /// Get the MongoDB database name.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="configKey"></param>
        /// <returns></returns>
        public static string GetMongoDbDatabase(this IConfiguration configuration, string configKey)
        {
            string val = configuration.GetValue<string>(configKey);
            if (string.IsNullOrEmpty(val))
                return configuration.GetMongoDbDatabase();
            return val;
        }
    }
}