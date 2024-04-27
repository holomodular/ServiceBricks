using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ServiceBricks.Xunit;

namespace ServiceBricks.Storage.EntityFrameworkCore.Xunit
{
    [ApiController]
    [Route("api/v{version:apiVersion}/EntityFrameworkCore/Example")]
    [Produces("application/json")]
    public class ExampleApiController : ApiController<ExampleDto>, IExampleApiController
    {
        public ExampleApiController(
            IApiService<ExampleDto> apiService,
            IOptions<ApiOptions> apiConfigOptions) : base(apiService, apiConfigOptions)
        {
        }
    }
}