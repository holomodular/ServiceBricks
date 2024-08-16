using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ServiceQuery;
using System.Net;

namespace ServiceBricks
{
    /// <summary>
    /// This is a REST API controller for a DTO requiring the admin security policy to invoke all methods.
    /// </summary>
    /// <typeparam name="TDto"></typeparam>
    public partial class AdminPolicyApiController<TDto> : ApiController<TDto>
        where TDto : class, new()
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="domainObjectService"></param>
        /// <param name="apiOptions"></param>
        public AdminPolicyApiController(
            IApiService<TDto> domainObjectService,
            IOptions<ApiOptions> apiOptions)
            : base(domainObjectService, apiOptions)
        {
        }

        /// <summary>
        /// Get
        /// </summary>
        /// <param name="storageKey"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{storageKey}")]
        [Route("Get/{storageKey}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [Authorize(Policy = ServiceBricksConstants.SECURITY_POLICY_ADMIN)]
        public override ActionResult Get([FromRoute] string storageKey)
        {
            return base.Get(storageKey);
        }

        /// <summary>
        /// Get
        /// </summary>
        /// <param name="storageKey"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        [Route("Get")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [Authorize(Policy = ServiceBricksConstants.SECURITY_POLICY_ADMIN)]
        public override ActionResult GetFromQuery([FromQuery] string storageKey)
        {
            return base.Get(storageKey);
        }

        /// <summary>
        /// Get
        /// </summary>
        /// <param name="storageKey"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAsync/{storageKey}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [Authorize(Policy = ServiceBricksConstants.SECURITY_POLICY_ADMIN)]
        public override async Task<ActionResult> GetAsync([FromRoute] string storageKey)
        {
            return await base.GetAsync(storageKey);
        }

        /// <summary>
        /// Get
        /// </summary>
        /// <param name="storageKey"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAsync")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [Authorize(Policy = ServiceBricksConstants.SECURITY_POLICY_ADMIN)]
        public override async Task<ActionResult> GetFromQueryAsync([FromQuery] string storageKey)
        {
            return await base.GetAsync(storageKey);
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("")]
        [Route("Update")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [Authorize(Policy = ServiceBricksConstants.SECURITY_POLICY_ADMIN)]
        public override ActionResult Update([FromBody] TDto dto)
        {
            return base.Update(dto);
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("UpdateAsync")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [Authorize(Policy = ServiceBricksConstants.SECURITY_POLICY_ADMIN)]
        public override async Task<ActionResult> UpdateAsync([FromBody] TDto dto)
        {
            return await base.UpdateAsync(dto);
        }

        /// <summary>
        /// Create
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        [Route("Create")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [Authorize(Policy = ServiceBricksConstants.SECURITY_POLICY_ADMIN)]
        public override ActionResult Create([FromBody] TDto dto)
        {
            return base.Create(dto);
        }

        /// <summary>
        /// Create
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CreateAsync")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [Authorize(Policy = ServiceBricksConstants.SECURITY_POLICY_ADMIN)]
        public override async Task<ActionResult> CreateAsync([FromBody] TDto dto)
        {
            return await base.CreateAsync(dto);
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="storageKey"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{storageKey}")]
        [Route("Delete/{storageKey}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [Authorize(Policy = ServiceBricksConstants.SECURITY_POLICY_ADMIN)]
        public override ActionResult Delete([FromRoute] string storageKey)
        {
            return base.Delete(storageKey);
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="storageKey"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("")]
        [Route("Delete")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [Authorize(Policy = ServiceBricksConstants.SECURITY_POLICY_ADMIN)]
        public override ActionResult DeleteFromQuery([FromQuery] string storageKey)
        {
            return base.Delete(storageKey);
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="storageKey"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("DeleteAsync/{storageKey}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [Authorize(Policy = ServiceBricksConstants.SECURITY_POLICY_ADMIN)]
        public override async Task<ActionResult> DeleteAsync([FromRoute] string storageKey)
        {
            return await base.DeleteAsync(storageKey);
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="storageKey"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("DeleteAsync")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [Authorize(Policy = ServiceBricksConstants.SECURITY_POLICY_ADMIN)]
        public override async Task<ActionResult> DeleteFromQueryAsync([FromQuery] string storageKey)
        {
            return await base.DeleteAsync(storageKey);
        }

        /// <summary>
        /// Query
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Query")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [Authorize(Policy = ServiceBricksConstants.SECURITY_POLICY_ADMIN)]
        public override ActionResult Query([FromBody] ServiceQueryRequest request)
        {
            return base.Query(request);
        }

        /// <summary>
        /// Query
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("QueryAsync")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [Authorize(Policy = ServiceBricksConstants.SECURITY_POLICY_ADMIN)]
        public override async Task<ActionResult> QueryAsync([FromBody] ServiceQueryRequest request)
        {
            return await base.QueryAsync(request);
        }
    }
}