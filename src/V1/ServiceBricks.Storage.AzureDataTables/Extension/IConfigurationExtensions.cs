using Microsoft.Extensions.Configuration;

namespace ServiceBricks.Storage.AzureDataTables
{
    /// <summary>
    /// Configuration extensions.
    /// </summary>
    public static partial class IConfigurationExtensions
    {
        /// <summary>
        /// Get the Azure Data Tables connection string.
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static string GetAzureDataTablesConnectionString(this IConfiguration configuration)
        {
            return configuration.GetValue<string>(StorageAzureDataTablesConstants.APPSETTING_CONNECTION_STRING);
        }

        /// <summary>
        /// Get the Azure Data Tables connection string.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="configKey"></param>
        /// <returns></returns>
        public static string GetAzureDataTablesConnectionString(this IConfiguration configuration, string configKey)
        {
            string val = configuration.GetValue<string>(configKey);
            if (string.IsNullOrEmpty(val))
                return configuration.GetValue<string>(StorageAzureDataTablesConstants.APPSETTING_CONNECTION_STRING);
            return val;
        }
    }
}