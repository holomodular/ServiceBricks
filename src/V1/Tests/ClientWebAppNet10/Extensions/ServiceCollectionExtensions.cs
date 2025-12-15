using Microsoft.Extensions.Options;
using ServiceBricks;
using static WebApp.Extensions.ServiceCollectionExtensions;

namespace WebApp.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomWebsite(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddLogging();
            services.AddHttpClient();
            services.AddControllers();
            services.AddRazorPages();
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddCors();
            services.AddMvc();

            services.AddScoped<IApiClient<ExampleDto>, ExampleApiClient>();

            return services;
        }

        

    }
}