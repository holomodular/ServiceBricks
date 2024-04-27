using Microsoft.Extensions.Logging;
using ServiceBricks.Xunit;

namespace ServiceBricks.Client.Xunit
{
    public class ExampleApiClient : ApiClient<ExampleDto>, IExampleApiClient
    {
        public ExampleApiClient(
            ILoggerFactory loggerFactory,
            IHttpClientFactory httpClientFactory,
            ApiConfig apiConfig)
            : base(loggerFactory, httpClientFactory, apiConfig)
        {
            ApiResource = $"EntityFrameworkCore/Example";
        }
    }
}