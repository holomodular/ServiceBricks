using Microsoft.Extensions.Logging;
using ServiceBricks.Xunit;

namespace ServiceBricks.Client.Xunit
{
    public class ExampleApiClient : ApiClient<ExampleDto>, IExampleApiClient
    {
        public ExampleApiClient(
            ILoggerFactory loggerFactory,
            IHttpClientFactory httpClientFactory,
            ClientApiOptions clientApiOptions)
            : base(loggerFactory, httpClientFactory, clientApiOptions)
        {
            ApiResource = $"EntityFrameworkCore/Example";
        }
    }
}