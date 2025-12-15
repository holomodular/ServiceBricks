using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ServiceBricks.Xunit;

namespace ServiceBricks.Storage.EntityFrameworkCore.Xunit
{
    public class ExampleApiClient : ApiClient<ExampleDto>, IExampleApiClient
    {
        //public ExampleApiClient(
        //    ILoggerFactory loggerFactory,
        //    IHttpClientFactory httpClientFactory,
        //    ClientApiOptions clientApiOptions)
        //    : base(loggerFactory, httpClientFactory, clientApiOptions)
        //{
        //    ApiResource = $"EntityFrameworkCore/Example";
        //}

        public ExampleApiClient(
    ILoggerFactory loggerFactory,
    IHttpClientFactory httpClientFactory,
    IConfiguration configuration)
    : base(loggerFactory, httpClientFactory, configuration.GetApiConfig())
        {
            ApiResource = @"Cache/CacheData";
        }

    }

    public class ExampleProcessQueueApiClient : ApiClient<ExampleWorkProcessDto>, IExampleProcessQueueApiClient
    {

    //    public ExampleProcessQueueApiClient(
    //ILoggerFactory loggerFactory,
    //IHttpClientFactory httpClientFactory,
    //ClientApiOptions clientApiOptions)
    //: base(loggerFactory, httpClientFactory, clientApiOptions)
    //    {
    //        ApiResource = $"EntityFrameworkCore/ExampleProcessQueue";
    //    }

        public ExampleProcessQueueApiClient(
            ILoggerFactory loggerFactory,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
            : base(loggerFactory, httpClientFactory, configuration.GetApiConfig())
        {
            ApiResource = $"EntityFrameworkCore/ExampleProcessQueue";
        }
    }
}