using Microsoft.Extensions.Configuration;

namespace ServiceBricks
{
    public static class ConfigurationExtensions
    {
        public static ClientApiOptions GetApiConfig(this IConfiguration configuration)
        {
            return GetApiConfig(configuration, null);
        }

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