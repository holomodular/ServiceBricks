using Microsoft.Extensions.Configuration;

namespace ServiceBricks
{
    /// <summary>
    /// IConfigurationBuilder extensions for the Core module.
    /// </summary>
    public static partial class ConfigurationBuilderExtensions
    {
        public const string APPSETTINGS_FILENAME = "appsettings.json";

        /// <summary>
        /// Add appsettings and environment variables.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IConfigurationBuilder AddAppSettingsConfig(this IConfigurationBuilder builder)
        {
            // Set Folder Path
            builder.SetBasePath(AppContext.BaseDirectory);

            // Add appsettings json file
            builder.AddJsonFile(APPSETTINGS_FILENAME, true);
            string[] fileSplit = APPSETTINGS_FILENAME.Split('.');
            if (fileSplit.Length == 2)
            {
                // Add appsettings environment json file
                var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                builder.AddJsonFile($"{fileSplit[0]}.{environmentName}.{fileSplit[1]}", true);
            }

            // Add Environment Variables
            builder.AddEnvironmentVariables();
            return builder;
        }
    }
}