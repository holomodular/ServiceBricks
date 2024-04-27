using Microsoft.Extensions.Configuration;

namespace ServiceBricks.Storage.MongoDb
{
    public static class IConfigurationExtensions
    {
        public static string GetMongoDbConnectionString(this IConfiguration configuration)
        {
            return configuration.GetValue<string>(StorageMongoDbConstants.APPSETTING_CONNECTION_STRING);
        }

        public static string GetMongoDbConnectionString(this IConfiguration configuration, string configKey)
        {
            string val = configuration.GetValue<string>(configKey);
            if (string.IsNullOrEmpty(val))
                return configuration.GetValue<string>(StorageMongoDbConstants.APPSETTING_CONNECTION_STRING);
            return val;
        }

        public static string GetMongoDbDatabaseName(this IConfiguration configuration)
        {
            return configuration.GetValue<string>(StorageMongoDbConstants.APPSETTING_DATABASE_NAME);
        }

        public static string GetMongoDbDatabaseName(this IConfiguration configuration, string configKey)
        {
            string val = configuration.GetValue<string>(configKey);
            if (string.IsNullOrEmpty(val))
                return configuration.GetValue<string>(StorageMongoDbConstants.APPSETTING_DATABASE_NAME);
            return val;
        }
    }
}