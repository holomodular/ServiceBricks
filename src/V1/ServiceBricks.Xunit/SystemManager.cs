using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.ComponentModel.Design.Serialization;
using System.Runtime.CompilerServices;

namespace ServiceBricks.Xunit
{
    public class ServiceContextSystemManager : ISystemManager
    {
        private readonly ServiceBricksSystemManager _rootSystem;
        private readonly IServiceScope _serviceScope;

        public ServiceContextSystemManager(ServiceBricksSystemManager rootSystem)
        {
            _rootSystem = rootSystem;
            _serviceScope = _rootSystem.ServiceProvider.CreateScope();
            ServiceProvider = _serviceScope.ServiceProvider;
        }

        public IServiceProvider ServiceProvider { get; set; }
    }

    public class ServiceBricksSystemManager : ISystemManager
    {
        private static ServiceBricksSystemManager _instance = null;
        private static object _instanceLock = new object();
        private static Type _currentType = null;

        private ServiceBricksSystemManager()
        {
        }

        public static ISystemManager GetSystemManager(Type startupType)
        {
            lock (_instanceLock)
            {
                if (_instance != null)
                {
                    if (startupType == _currentType)
                        return new ServiceContextSystemManager(_instance);
                    else
                    {
                        _instance.StopSystem();
                        _instance = null;
                    }
                }

                _currentType = startupType;
                _instance = new ServiceBricksSystemManager();
                _instance.StartSystem(startupType);
                return new ServiceContextSystemManager(_instance);
            }
        }

        public IServiceProvider ServiceProvider { get; set; }
        public IConfiguration Configuration { get; set; }
        public TestServer TestServer { get; set; }

        private CancellationTokenSource CancellationTokenSource;
        private SemaphoreSlim _signal = new SemaphoreSlim(0);

        public virtual IWebHostBuilder CreateWebHostBuilder(Type startupType)
        {
            return new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration(config =>
                {
                    config.AddAppSettingsConfig();
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.ClearProviders();
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                    logging.AddDebug();
                })
                .UseEnvironment("Development")
                .UseStartup(startupType);
        }

        public virtual void StopSystem()
        {
            CancellationTokenSource?.Cancel();
            TestServer.Dispose();
        }

        public virtual void StartSystem(Type startupType)
        {
            ServiceBricksStartup.ConfigureCompleteEvent += Startup_ConfigureCompleteEvent;

            // Create host builder
            var webHostBuilder = CreateWebHostBuilder(startupType);

            //var build = webHostBuilder.Build();
            //ServiceProvider = build.Services;
            //Configuration = (IConfiguration)build.Services.GetService(typeof(IConfiguration));

            TestServer = new Microsoft.AspNetCore.TestHost.TestServer(webHostBuilder);
            ServiceProvider = TestServer.Services;
            Configuration = (IConfiguration)TestServer.Host.Services.GetService(typeof(IConfiguration));

            CancellationTokenSource = new CancellationTokenSource();

            _signal.Wait(CancellationTokenSource.Token);
        }

        public virtual void Startup_ConfigureCompleteEvent(IApplicationBuilder app)
        {
            _signal.Release();
        }
    }
}