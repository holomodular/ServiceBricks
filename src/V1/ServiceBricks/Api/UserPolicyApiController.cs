using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ServiceQuery;
using System.Net;

namespace ServiceBricks
{
    /// <summary>
    /// This is a REST-based API controller for a domain object
    /// requiring the user security policy to invoke all methods.
    /// </summary>
    /// <typeparam name="TDto"></typeparam>
    public class UserPolicyApiController<TDto> : ApiController<TDto>
        where TDto : class, new()
    {
        public UserPolicyApiController(
            IApiService<TDto> domainObjectService,
            IOptions<ApiOptions> apiOptions)
            : base(domainObjectService, apiOptions)
        {
        }

        [HttpGet]
        [Route("{storageKey}")]
        [Route("Get/{storageKey}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [Authorize(Policy = ServiceBricksConstants.SECURITY_POLICY_USER)]
        public override ActionResult Get([FromRoute] string storageKey)
        {
            return base.Get(storageKey);
        }

        [HttpGet]
        [Route("")]
        [Route("Get")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [Authorize(Policy = ServiceBricksConstants.SECURITY_POLICY_USER)]
        public override ActionResult GetFromQuery([FromQuery] string storageKey)
        {
            return base.Get(storageKey);
        }

        [HttpGet]
        [Route("GetAsync/{storageKey}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [Authorize(Policy = ServiceBricksConstants.SECURITY_POLICY_USER)]
        public override async Task<ActionResult> GetAsync([FromRoute] string storageKey)
        {
            return await base.GetAsync(storageKey);
        }

        [HttpGet]
        [Route("GetAsync")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [Authorize(Policy = ServiceBricksConstants.SECURITY_POLICY_USER)]
        public override async Task<ActionResult> GetFromQueryAsync([FromQuery] string storageKey)
        {
            return await base.GetAsync(storageKey);
        }

        [HttpPut]
        [Route("")]
        [Route("Update")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [Authorize(Policy = ServiceBricksConstants.SECURITY_POLICY_USER)]
        public override ActionResult Update([FromBody] TDto dto)
        {
            return base.Update(dto);
        }

        [HttpPut]
        [Route("UpdateAsync")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [Authorize(Policy = ServiceBricksConstants.SECURITY_POLICY_USER)]
        public override async Task<ActionResult> UpdateAsync([FromBody] TDto dto)
        {
            return await base.UpdateAsync(dto);
        }

        [HttpPost]
        [Route("")]
        [Route("Create")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [Authorize(Policy = ServiceBricksConstants.SECURITY_POLICY_USER)]
        public override ActionResult Create([FromBody] TDto dto)
        {
            return base.Create(dto);
        }

        [HttpPost]
        [Route("CreateAsync")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [Authorize(Policy = ServiceBricksConstants.SECURITY_POLICY_USER)]
        public override async Task<ActionResult> CreateAsync([FromBody] TDto dto)
        {
            return await base.CreateAsync(dto);
        }

        [HttpDelete]
        [Route("{storageKey}")]
        [Route("Delete/{storageKey}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [Authorize(Policy = ServiceBricksConstants.SECURITY_POLICY_USER)]
        public override ActionResult Delete([FromRoute] string storageKey)
        {
            return base.Delete(storageKey);
        }

        [HttpDelete]
        [Route("")]
        [Route("Delete")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [Authorize(Policy = ServiceBricksConstants.SECURITY_POLICY_USER)]
        public override ActionResult DeleteFromQuery([FromQuery] string storageKey)
        {
            return base.Delete(storageKey);
        }

        [HttpDelete]
        [Route("DeleteAsync/{storageKey}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [Authorize(Policy = ServiceBricksConstants.SECURITY_POLICY_USER)]
        public override async Task<ActionResult> DeleteAsync([FromRoute] string storageKey)
        {
            return await base.DeleteAsync(storageKey);
        }

        [HttpDelete]
        [Route("DeleteAsync")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [Authorize(Policy = ServiceBricksConstants.SECURITY_POLICY_USER)]
        public override async Task<ActionResult> DeleteFromQueryAsync([FromQuery] string storageKey)
        {
            return await base.DeleteAsync(storageKey);
        }

        [HttpPost]
        [Route("Query")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [Authorize(Policy = ServiceBricksConstants.SECURITY_POLICY_USER)]
        public override ActionResult Query([FromBody] ServiceQueryRequest request)
        {
            return base.Query(request);
        }

        [HttpPost]
        [Route("QueryAsync")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [Authorize(Policy = ServiceBricksConstants.SECURITY_POLICY_USER)]
        public override async Task<ActionResult> QueryAsync([FromBody] ServiceQueryRequest request)
        {
            return await base.QueryAsync(request);
        }
    }
}