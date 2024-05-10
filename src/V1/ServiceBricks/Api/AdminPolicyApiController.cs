﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ServiceQuery;
using System.Net;

namespace ServiceBricks
{
    /// <summary>
    /// This is a REST-based API controller for a domain object
    /// requiring the admin security policy to invoke all methods.
    /// </summary>
    /// <typeparam name="TDto"></typeparam>
    public class AdminPolicyApiController<TDto> : ApiController<TDto>
        where TDto : class, new()
    {
        public AdminPolicyApiController(
            IApiService<TDto> domainObjectService,
            IOptions<ApiOptions> apiOptions)
            : base(domainObjectService, apiOptions)
        {
        }

        [HttpGet]
        [Route("Get")]
        [Route("Get/{storageKey}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [Authorize(Policy = ServiceBricksConstants.SECURITY_POLICY_ADMIN)]
        public override ActionResult Get([FromQuery] string storageKey)
        {
            return base.Get(storageKey);
        }

        [HttpGet]
        [Route("GetAsync")]
        [Route("GetAsync/{storageKey}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [Authorize(Policy = ServiceBricksConstants.SECURITY_POLICY_ADMIN)]
        public override async Task<ActionResult> GetAsync([FromQuery] string storageKey)
        {
            return await base.GetAsync(storageKey);
        }

        [HttpPut]
        [Route("Update")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [Authorize(Policy = ServiceBricksConstants.SECURITY_POLICY_ADMIN)]
        public override ActionResult Update([FromBody] TDto dto)
        {
            return base.Update(dto);
        }

        [HttpPut]
        [Route("UpdateAsync")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [Authorize(Policy = ServiceBricksConstants.SECURITY_POLICY_ADMIN)]
        public override async Task<ActionResult> UpdateAsync([FromBody] TDto dto)
        {
            return await base.UpdateAsync(dto);
        }

        [HttpPost]
        [Route("Create")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [Authorize(Policy = ServiceBricksConstants.SECURITY_POLICY_ADMIN)]
        public override ActionResult Create([FromBody] TDto dto)
        {
            return base.Create(dto);
        }

        [HttpPost]
        [Route("CreateAsync")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [Authorize(Policy = ServiceBricksConstants.SECURITY_POLICY_ADMIN)]
        public override async Task<ActionResult> CreateAsync([FromBody] TDto dto)
        {
            return await base.CreateAsync(dto);
        }

        [HttpDelete]
        [Route("Delete")]
        [Route("Delete/{storageKey}")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [Authorize(Policy = ServiceBricksConstants.SECURITY_POLICY_ADMIN)]
        public override ActionResult Delete([FromQuery] string storageKey)
        {
            return base.Delete(storageKey);
        }

        [HttpDelete]
        [Route("DeleteAsync")]
        [Route("DeleteAsync/{storageKey}")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [Authorize(Policy = ServiceBricksConstants.SECURITY_POLICY_ADMIN)]
        public override async Task<ActionResult> DeleteAsync([FromQuery] string storageKey)
        {
            return await base.DeleteAsync(storageKey);
        }

        [HttpPost]
        [Route("Query")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [Authorize(Policy = ServiceBricksConstants.SECURITY_POLICY_ADMIN)]
        public override ActionResult Query([FromBody] ServiceQueryRequest request)
        {
            return base.Query(request);
        }

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