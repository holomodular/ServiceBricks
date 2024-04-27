using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ServiceBricks.Xunit;

namespace ServiceBricks.Storage.EntityFrameworkCore.Xunit
{
    public class ExampleApiClient : ApiClient<ExampleDto>, IExampleApiClient
    {
        public ExampleApiClient(
            ILoggerFactory loggerFactory,
            IHttpClientFactory httpClientFactory,
            IOptions<ApiConfig> apiConfigOptions)
            : base(loggerFactory, httpClientFactory, apiConfigOptions.Value)
        {
        }
    }
}