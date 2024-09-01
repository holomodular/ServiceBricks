using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ServiceBricks.Xunit;

namespace ServiceBricks.Client.Xunit
{
    public class ExampleApiClient : ApiClient<ExampleDto>, IExampleApiClient
    {
        public ExampleApiClient(
            ILoggerFactory loggerFactory,
            IHttpClientFactory httpClientFactory,
            IOptions<ClientApiOptions> clientApiOptions)
            : base(loggerFactory, httpClientFactory, clientApiOptions.Value)
        {
            ApiResource = $"EntityFrameworkCore/Example";
        }
    }
}