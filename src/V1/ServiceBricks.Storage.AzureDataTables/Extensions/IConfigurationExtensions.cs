using Microsoft.Extensions.Configuration;

namespace ServiceBricks.Storage.AzureDataTables
{
    public static class IConfigurationExtensions
    {
        public static string GetAzureDataTablesConnectionString(this IConfiguration configuration)
        {
            return configuration.GetValue<string>(StorageAzureDataTablesConstants.APPSETTING_CONNECTION_STRING);
        }

        public static string GetAzureDataTablesConnectionString(this IConfiguration configuration, string configKey)
        {
            string val = configuration.GetValue<string>(configKey);
            if (string.IsNullOrEmpty(val))
                return configuration.GetValue<string>(StorageAzureDataTablesConstants.APPSETTING_CONNECTION_STRING);
            return val;
        }
    }
}