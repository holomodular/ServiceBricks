using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace WebApp.Controllers.api.v1
{
    [AllowAnonymous]
    [ApiController]
    [ApiVersion("1.0")]
    [Produces("application/json", "application/problem+json")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ExampleController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(List<int>), (int)HttpStatusCode.OK)]
        [Route("GetFiveNumbers")]
        public IActionResult GetFiveNumbers()
        {
            return Ok(
                Enumerable.Range(1, 5).Select(x =>
                    Random.Shared.Next(0, 100)).ToList());
        }

        [HttpPost]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [Route("ThrowNotSupportedException")]
        public IActionResult ThrowNotSupportedException()
        {
            throw new NotSupportedException(nameof(ThrowNotSupportedException));
        }
    }
}