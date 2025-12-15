using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ServiceBricks.TestDataTypes
{
    /// <summary>
    /// This is a REST API Controller for TestDto.
    /// </summary>
    [ApiController]
    [Route("api/v{version:apiVersion}/TestDataTypes/Test")]
    [Produces("application/json")]
    public partial class TestApiController : AdminPolicyApiController<TestDto>, ITestApiController
    {
        protected readonly ITestApiService _apiService;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="apiService"></param>
        /// <param name="apiOptions"></param>
        public TestApiController(
            ITestApiService apiService,
            IOptions<ApiOptions> apiOptions)
            : base(apiService, apiOptions)
        {
            _apiService = apiService;
        }
    }
}
