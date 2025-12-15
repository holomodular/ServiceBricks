using Microsoft.Extensions.Options;
using ServiceBricks;

namespace WebApp
{
    public interface IExampleApiClient : IApiClient<ExampleDto>
    {
    }

    public class ExampleDto : DataTransferObject
    {
        public string Name { get; set; }
        public DateTimeOffset CreateDate { get; set; }
        public DateTimeOffset UpdateDate { get; set; }
        public DateTimeOffset ExampleDate { get; set; }
        public DateTimeOffset? ExampleNullableDate { get; set; }
        public DateTimeOffset? ExampleNullableDateNotSet { get; set; }
        public DateTime SimpleDate { get; set; }
        public DateTime SimpleNullableDate { get; set; }
        public DateTime? SimpleNullableDateNotSet { get; set; }
    }

    public class ExampleApiClient : ApiClient<ExampleDto>, IExampleApiClient
    {
        public ExampleApiClient(
            ILoggerFactory loggerFactory,
            IHttpClientFactory httpClientFactory)
            : base(loggerFactory, httpClientFactory, new ClientApiOptions()
            {
                BaseServiceUrl = "https://localhost:7000/api/v1.0",
                DisableAuthentication = true,
                ReturnResponseObject = true
            })
        {
            ApiResource = $"EntityFrameworkCore/Example";
        }
    }
}
