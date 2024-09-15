using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ServiceBricks.Xunit;
using Newtonsoft.Json;
using ServiceQuery;
using Microsoft.AspNetCore.Mvc;
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
            var options = new OptionsWrapper<ClientApiOptions>(new ClientApiOptions() { ReturnResponseObject = false, BaseServiceUrl = "https://localhost:7000/", TokenUrl = "https://localhost:7000/token" });
            var handler = new ApiClientTests.CustomGenericHttpClientHandler<ExampleDto>(controller);
            var clientHandlerFactory = new ExampleHttpClientFactory(handler);
            return new ExampleApiClient(
                serviceProvider.GetRequiredService<ILoggerFactory>(),
                clientHandlerFactory,
                options);
        }

        public ApiClientTests.CustomGenericHttpClientHandler<ExampleDto> Handler { get; set; }

        public override IApiClient<ExampleDto> GetClientReturnResponse(IServiceProvider serviceProvider)
        {
            var apioptions = new OptionsWrapper<ApiOptions>(new ApiOptions() { ReturnResponseObject = true });
            var apiservice = serviceProvider.GetRequiredService<IApiService<ExampleDto>>();
            var controller = new ExampleApiController(apiservice, apioptions);

            var options = new OptionsWrapper<ClientApiOptions>(new ClientApiOptions() { ReturnResponseObject = true, BaseServiceUrl = "https://localhost:7000/", TokenUrl = "https://localhost:7000/token" });
            var handler = new ApiClientTests.CustomGenericHttpClientHandler<ExampleDto>(controller);
            var clientHandlerFactory = new ExampleHttpClientFactory(handler);
            return new ExampleApiClient(
                serviceProvider.GetRequiredService<ILoggerFactory>(),
                clientHandlerFactory,
                options);
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

    public class StubExampleApiClient : ApiClient<ExampleDto>, IExampleApiClient
    {
        public StubExampleApiClient(
            ILoggerFactory loggerFactory,
            IHttpClientFactory httpClientFactory,
            IOptions<ClientApiOptions> clientApiOptions)
            : base(loggerFactory, httpClientFactory, clientApiOptions.Value)
        {
            ApiResource = $"EntityFrameworkCore/Example";
        }
    }
}