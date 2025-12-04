using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace WebApp.Controllers.api.v2
{
    [AllowAnonymous]
    [ApiController]
    [ApiVersion("2.0")]
    [Produces("application/json", "application/problem+json")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ExampleController : v1.ExampleController
    {
        [HttpGet]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(List<int>), (int)HttpStatusCode.OK)]
        [Route("GetTenNumbers")]
        public IActionResult GetTenNumbers()
        {
            return Ok(
                Enumerable.Range(1, 10).Select(x =>
                    Random.Shared.Next(0, 100)).ToList());
        }
    }
}