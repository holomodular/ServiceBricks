using Microsoft.Extensions.Configuration;

namespace ServiceBricks
{
    public static class ConfigurationExtensions
    {
        public static ApiConfig GetApiConfig(this IConfiguration configuration)
        {
            return GetApiConfig(configuration, null);
        }

        public static ApiConfig GetApiConfig(this IConfiguration configuration, string configKey)
        {
            IConfigurationSection section = null;
            ApiConfig config = null;
            if (!string.IsNullOrEmpty(configKey))
            {
                section = configuration.GetSection(configKey);
                config = section.Get<ApiConfig>();
                if (config != null)
                    return config;
            }

            section = configuration.GetSection(ServiceBricksConstants.APPSETTING_CLIENT_APICONFIG);
            config = section.Get<ApiConfig>();
            if (config != null)
                return config;
            return null;
        }
    }
}