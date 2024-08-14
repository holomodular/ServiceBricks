using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ServiceBricks
{
    /// <summary>
    /// The startup class
    /// </summary>
    public abstract partial class Startup
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// The configuration
        /// </summary>
        public static IConfiguration Configuration { get; set; }

        /// <summary>
        /// Custom event when ConfigureServices is complete
        /// </summary>
        public static event ConfigureServicesCompleteHandler ConfigureServicesCompleteEvent;

        /// <summary>
        /// Custom event when Configure is complete
        /// </summary>
        public static event ConfigureCompleteHandler ConfigureCompleteEvent;

        /// <summary>
        /// Handler for when ConfigureServices is complete
        /// </summary>
        /// <param name="services"></param>
        public delegate void ConfigureServicesCompleteHandler(IServiceCollection services);

        /// <summary>
        /// Handler for when Configure is complete
        /// </summary>
        /// <param name="app"></param>
        public delegate void ConfigureCompleteHandler(IApplicationBuilder app);

        /// <summary>
        /// Add bricks
        /// </summary>
        /// <param name="services"></param>
        public virtual void AddBricks(IServiceCollection services)
        {
        }

        /// <summary>
        /// Start bricks
        /// </summary>
        /// <param name="app"></param>
        public virtual void StartBricks(IApplicationBuilder app)
        {
        }

        /// <summary>
        /// Custom configure services
        /// </summary>
        /// <param name="services"></param>
        public virtual void CustomConfigureServices(IServiceCollection services)
        {
            AddBricks(services);

            // Custom event when complete
            if (ConfigureServicesCompleteEvent != null)
                ConfigureServicesCompleteEvent(services);
        }

        /// <summary>
        /// Custom configure
        /// </summary>
        /// <param name="app"></param>
        public virtual void CustomConfigure(IApplicationBuilder app)
        {
            StartBricks(app);

            // Custom event when complete
            if (ConfigureCompleteEvent != null)
                ConfigureCompleteEvent(app);
        }
    }
}