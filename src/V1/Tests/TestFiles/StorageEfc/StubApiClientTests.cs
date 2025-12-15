using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ServiceBricks.Storage.EntityFrameworkCore.Xunit;

namespace ServiceBricks.Xunit
{
    public class StubTestManager : ExampleTestManager
    {
        public class ExampleHttpClientFactory : IHttpClientFactory
        {
            private ApiClientTests.CustomGenericHttpClientHandler<ExampleDto> _handler;

            public ExampleHttpClientFactory(ApiClientTests.CustomGenericHttpClientHandler<ExampleDto> handler)
            {
                _handler = handler;
            }

            public HttpClient CreateClient(string name)
            {
                return new HttpClient(_handler);
            }
        }

        public override IApiClient<ExampleDto> GetClient(IServiceProvider serviceProvider)
        {
            var apioptions = new OptionsWrapper<ApiOptions>(new ApiOptions() { ReturnResponseObject = false });
            var apiservice = serviceProvider.GetRequiredService<IApiService<ExampleDto>>();
            var controller = new ExampleApiController(apiservice, apioptions);
            
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    { ServiceBricksConstants.APPSETTING_CLIENT_APIOPTIONS + ":ReturnResponseObject", "false" },
                    { ServiceBricksConstants.APPSETTING_CLIENT_APIOPTIONS + ":DisableAuthentication", "false" },
                    { ServiceBricksConstants.APPSETTING_CLIENT_APIOPTIONS + ":TokenUrl", "https://localhost:7000/token" },
                    { ServiceBricksConstants.APPSETTING_CLIENT_APIOPTIONS + ":BaseServiceUrl", "https://localhost:7000/" },
                })
                .Build();

            var handler = new ApiClientTests.CustomGenericHttpClientHandler<ExampleDto>(controller);
            var clientHandlerFactory = new ExampleHttpClientFactory(handler);
            return new ExampleApiClient(
                serviceProvider.GetRequiredService<ILoggerFactory>(),
                clientHandlerFactory,
                config);
        }

        public ApiClientTests.CustomGenericHttpClientHandler<ExampleDto> Handler { get; set; }

        public override IApiClient<ExampleDto> GetClientReturnResponse(IServiceProvider serviceProvider)
        {
            var apioptions = new OptionsWrapper<ApiOptions>(new ApiOptions() { ReturnResponseObject = true });
            var apiservice = serviceProvider.GetRequiredService<IApiService<ExampleDto>>();
            var controller = new ExampleApiController(apiservice, apioptions);
            
            var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                            { ServiceBricksConstants.APPSETTING_CLIENT_APIOPTIONS + ":ReturnResponseObject", "true" },
                            { ServiceBricksConstants.APPSETTING_CLIENT_APIOPTIONS + ":DisableAuthentication", "false" },
                            { ServiceBricksConstants.APPSETTING_CLIENT_APIOPTIONS + ":TokenUrl", "https://localhost:7000/token" },
                            { ServiceBricksConstants.APPSETTING_CLIENT_APIOPTIONS + ":BaseServiceUrl", "https://localhost:7000/" },
            })
            .Build();

            var handler = new ApiClientTests.CustomGenericHttpClientHandler<ExampleDto>(controller);
            var clientHandlerFactory = new ExampleHttpClientFactory(handler);
            return new ExampleApiClient(
                serviceProvider.GetRequiredService<ILoggerFactory>(),
                clientHandlerFactory,
                config);
        }
    }

    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public class StubExampleApiClientTest : ApiClientTest<ExampleDto>
    {
        public StubExampleApiClientTest()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(EntityFrameworkCoreStartup));
            TestManager = new StubTestManager();
        }
    }

    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public class StubExampleApiClientReturnResponseTests : ApiClientReturnResponseTest<ExampleDto>
    {
        public StubExampleApiClientReturnResponseTests()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(EntityFrameworkCoreStartup));
            TestManager = new StubTestManager();
        }
    }

}