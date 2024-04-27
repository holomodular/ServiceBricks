//using Microsoft.AspNetCore.Builder;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;

//namespace ServiceBricks.Xunit
//{
//    public class TestStartup : ServiceBricks.Startup
//    {
//        public TestStartup(IConfiguration configuration) : base(configuration)
//        {
//        }

//        public virtual void ConfigureDevelopmentServices(IServiceCollection services)
//        {
//            base.CustomConfigureServices(services);
//            services.AddSingleton(Configuration);
//            services.AddServiceBricks(Configuration);

//            // Remove all background tasks/timers for unit testing

//            // Register TestManagers

//            services.AddServiceBricksComplete();
//        }

//        public virtual void Configure(IApplicationBuilder app)
//        {
//            base.CustomConfigure(app);
//            app.StartServiceBricks();
//        }
//    }
//}