using Microsoft.Extensions.Configuration;

namespace ServiceBricks
{
    /// <summary>
    /// IConfiguration extensions for the Core module.
    /// </summary>
    public static partial class ConfigurationExtensions
    {
        /// <summary>
        /// Get the API configuration.
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static ClientApiOptions GetApiConfig(this IConfiguration configuration)
        {
            return GetApiConfig(configuration, null);
        }

        /// <summary>
        /// Get the API configuration.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="configKey"></param>
        /// <returns></returns>
        public static ClientApiOptions GetApiConfig(this IConfiguration configuration, string configKey)
        {
            IConfigurationSection section = null;
            ClientApiOptions config = null;
            if (!string.IsNullOrEmpty(configKey))
            {
                section = configuration.GetSection(configKey);
                config = section.Get<ClientApiOptions>();
                if (config != null)
                    return config;
            }

            section = configuration.GetSection(ServiceBricksConstants.APPSETTING_CLIENT_APIOPTIONS);
            config = section.Get<ClientApiOptions>();
            if (config != null)
                return config;
            return null;
        }
    }
}