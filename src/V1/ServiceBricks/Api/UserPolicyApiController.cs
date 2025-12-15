using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ServiceQuery;
using System.Net;

namespace ServiceBricks
{
    /// <summary>
    /// This is a REST API controller for a DTO requiring the user security policy to invoke all methods.
    /// </summary>
    /// <typeparam name="TDto"></typeparam>
    public partial class UserPolicyApiController<TDto> : ApiController<TDto>
        where TDto : class, IDataTransferObject, new()
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="domainObjectService"></param>
        /// <param name="apiOptions"></param>
        public UserPolicyApiController(
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
        [Authorize(Policy = ServiceBricksConstants.SECURITY_POLICY_USER)]
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
        [Authorize(Policy = ServiceBricksConstants.SECURITY_POLICY_USER)]
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
        [Authorize(Policy = ServiceBricksConstants.SECURITY_POLICY_USER)]
        public override async Task<ActionResult> GetAsync([FromRoute] string storageKey, CancellationToken cancellationToken = default)
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
        [Authorize(Policy = ServiceBricksConstants.SECURITY_POLICY_USER)]
        public override async Task<ActionResult> GetFromQueryAsync([FromQuery] string storageKey, CancellationToken cancellationToken = default)
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
        [Authorize(Policy = ServiceBricksConstants.SECURITY_POLICY_USER)]
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
        [Authorize(Policy = ServiceBricksConstants.SECURITY_POLICY_USER)]
        public override async Task<ActionResult> UpdateAsync([FromBody] TDto dto, CancellationToken cancellationToken = default)
        {
            return await base.UpdateAsync(dto);
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("UpdateAck")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [Authorize(Policy = ServiceBricksConstants.SECURITY_POLICY_USER)]
        public override ActionResult UpdateAck([FromBody] TDto dto)
        {
            return base.UpdateAck(dto);
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("UpdateAckAsync")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [Authorize(Policy = ServiceBricksConstants.SECURITY_POLICY_USER)]
        public override async Task<ActionResult> UpdateAckAsync([FromBody] TDto dto, CancellationToken cancellationToken = default)
        {
            return await base.UpdateAckAsync(dto);
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
        [Authorize(Policy = ServiceBricksConstants.SECURITY_POLICY_USER)]
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
        [Authorize(Policy = ServiceBricksConstants.SECURITY_POLICY_USER)]
        public override async Task<ActionResult> CreateAsync([FromBody] TDto dto, CancellationToken cancellationToken = default)
        {
            return await base.CreateAsync(dto);
        }

        /// <summary>
        /// Create
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CreateAck")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [Authorize(Policy = ServiceBricksConstants.SECURITY_POLICY_USER)]
        public override ActionResult CreateAck([FromBody] TDto dto)
        {
            return base.CreateAck(dto);
        }

        /// <summary>
        /// Create
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CreateAckAsync")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [Authorize(Policy = ServiceBricksConstants.SECURITY_POLICY_USER)]
        public override async Task<ActionResult> CreateAckAsync([FromBody] TDto dto, CancellationToken cancellationToken = default)
        {
            return await base.CreateAckAsync(dto);
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
        [Authorize(Policy = ServiceBricksConstants.SECURITY_POLICY_USER)]
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
        [Authorize(Policy = ServiceBricksConstants.SECURITY_POLICY_USER)]
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
        [Authorize(Policy = ServiceBricksConstants.SECURITY_POLICY_USER)]
        public override async Task<ActionResult> DeleteAsync([FromRoute] string storageKey, CancellationToken cancellationToken = default)
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
        [Authorize(Policy = ServiceBricksConstants.SECURITY_POLICY_USER)]
        public override async Task<ActionResult> DeleteFromQueryAsync([FromQuery] string storageKey, CancellationToken cancellationToken = default)
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
        [Authorize(Policy = ServiceBricksConstants.SECURITY_POLICY_USER)]
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
        [Authorize(Policy = ServiceBricksConstants.SECURITY_POLICY_USER)]
        public override async Task<ActionResult> QueryAsync([FromBody] ServiceQueryRequest request, CancellationToken cancellationToken = default)
        {
            return await base.QueryAsync(request);
        }

        /// <summary>
        /// Patch
        /// </summary>
        /// <param name="storageKey"></param>
        /// <param name="patchDocument"></param>
        /// <returns></returns>
        [HttpPatch]
        [Route("{storageKey}")]
        [Route("Patch/{storageKey}")]
        [Consumes("application/json-patch+json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [Authorize(Policy = ServiceBricksConstants.SECURITY_POLICY_USER)]
        public override ActionResult Patch(
            [FromRoute] string storageKey,
            [FromBody] JsonPatchDocument<TDto> patchDocument)
        {
            return base.Patch(storageKey, patchDocument);
        }

        /// <summary>
        /// Patch
        /// </summary>
        /// <param name="storageKey"></param>
        /// <param name="patchDocument"></param>
        /// <returns></returns>
        [HttpPatch]
        [Route("")]
        [Route("Patch")]
        [Consumes("application/json-patch+json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [Authorize(Policy = ServiceBricksConstants.SECURITY_POLICY_USER)]
        public override ActionResult PatchFromQuery(
            [FromQuery] string storageKey,
            [FromBody] JsonPatchDocument<TDto> patchDocument)
        {
            return base.Patch(storageKey, patchDocument);
        }


        /// <summary>
        /// Patch
        /// </summary>
        /// <param name="storageKey"></param>
        /// <param name="patchDocument"></param>
        /// <returns></returns>
        [HttpPatch]
        [Route("PatchAsync/{storageKey}")]
        [Consumes("application/json-patch+json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [Authorize(Policy = ServiceBricksConstants.SECURITY_POLICY_USER)]
        public override async Task<ActionResult> PatchAsync(
            [FromRoute] string storageKey,
            [FromBody] JsonPatchDocument<TDto> patchDocument,
            CancellationToken cancellationToken = default)
        {
            return await base.PatchAsync(storageKey, patchDocument);
        }

        /// <summary>
        /// Patch
        /// </summary>
        /// <param name="storageKey"></param>
        /// <param name="patchDocument"></param>
        /// <returns></returns>
        [HttpPatch]
        [Route("PatchAsync")]
        [Consumes("application/json-patch+json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [Authorize(Policy = ServiceBricksConstants.SECURITY_POLICY_USER)]
        public override async Task<ActionResult> PatchFromQueryAsync(
            [FromQuery] string storageKey,
            [FromBody] JsonPatchDocument<TDto> patchDocument,
            CancellationToken cancellationToken = default)
        {
            return await base.PatchAsync(storageKey, patchDocument);
        }


        /// <summary>
        /// Patch
        /// </summary>
        /// <param name="storageKey"></param>
        /// <param name="patchDocument"></param>
        /// <returns></returns>
        [HttpPatch]
        [Route("PatchAck/{storageKey}")]
        [Consumes("application/json-patch+json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [Authorize(Policy = ServiceBricksConstants.SECURITY_POLICY_USER)]
        public override ActionResult PatchAck(
            [FromRoute] string storageKey,
            [FromBody] JsonPatchDocument<TDto> patchDocument)
        {
            return base.PatchAck(storageKey, patchDocument);
        }

        /// <summary>
        /// Patch
        /// </summary>
        /// <param name="storageKey"></param>
        /// <param name="patchDocument"></param>
        /// <returns></returns>
        [HttpPatch]
        [Route("PatchAck")]
        [Consumes("application/json-patch+json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [Authorize(Policy = ServiceBricksConstants.SECURITY_POLICY_USER)]
        public override ActionResult PatchAckFromQuery(
            [FromQuery] string storageKey,
            [FromBody] JsonPatchDocument<TDto> patchDocument)
        {
            return PatchAck(storageKey, patchDocument);
        }


        /// <summary>
        /// Patch
        /// </summary>
        /// <param name="storageKey"></param>
        /// <param name="patchDocument"></param>
        /// <returns></returns>
        [HttpPatch]
        [Route("PatchAckAsync/{storageKey}")]
        [Consumes("application/json-patch+json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [Authorize(Policy = ServiceBricksConstants.SECURITY_POLICY_USER)]
        public override async Task<ActionResult> PatchAckAsync(
            [FromRoute] string storageKey,
            [FromBody] JsonPatchDocument<TDto> patchDocument,
            CancellationToken cancellationToken = default)
        {
            return await base.PatchAckAsync(storageKey, patchDocument);
        }

        /// <summary>
        /// Patch
        /// </summary>
        /// <param name="storageKey"></param>
        /// <param name="patchDocument"></param>
        /// <returns></returns>
        [HttpPatch]
        [Route("PatchAckAsync")]
        [Consumes("application/json-patch+json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [Authorize(Policy = ServiceBricksConstants.SECURITY_POLICY_USER)]
        public override async Task<ActionResult> PatchAckFromQueryAsync(
            [FromQuery] string storageKey,
            [FromBody] JsonPatchDocument<TDto> patchDocument,
            CancellationToken cancellationToken = default)
        {
            return await base.PatchAckAsync(storageKey, patchDocument);
        }

        /// <summary>
        /// Validate
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Validate")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [Authorize(Policy = ServiceBricksConstants.SECURITY_POLICY_USER)]
        public override ActionResult Validate([FromBody] TDto dto)
        {
            return base.Validate(dto);
        }

        /// <summary>
        /// Validate
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ValidateAsync")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [Authorize(Policy = ServiceBricksConstants.SECURITY_POLICY_USER)]
        public override async Task<ActionResult> ValidateAsync([FromBody] TDto dto, CancellationToken cancellationToken = default)
        {
            return await base.ValidateAsync(dto, cancellationToken);
        }
    }
}