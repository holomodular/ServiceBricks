using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ServiceBricks.TestDataTypes
{
    /// <summary>
    /// This class is an REST API client for the TestDto.
    /// </summary>
    public partial class TestApiClient : ApiClient<TestDto>, ITestApiClient
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="loggerFactory"></param>
        /// <param name="httpClientFactory"></param>
        /// <param name="configuration"></param>
        public TestApiClient(
            ILoggerFactory loggerFactory,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
            : base(loggerFactory, httpClientFactory, configuration.GetApiConfig(TestDataTypesModelConstants.APPSETTING_CLIENT_APICONFIG))
        {
            ApiResource = @"TestDataTypes/Test";
        }
    }
}
