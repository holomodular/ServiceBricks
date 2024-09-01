using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using ServiceQuery;
using System.Net;

namespace ServiceBricks
{
    /// <summary>
    /// This is a REST API controller for a domain object.
    /// By default, no security policy is added to this base class. Use AdminPolicyRestApiController instead.
    /// </summary>
    /// <typeparam name="TDto"></typeparam>
    public partial class ApiController<TDto> : ControllerBase, IApiController<TDto>
        where TDto : class
    {
        protected readonly IApiService<TDto> _domainObjectService;
        protected readonly ApiOptions _apiOptions;

        public ApiController(
            IApiService<TDto> domainObjectService,
            IOptions<ApiOptions> apiOptions)
        {
            _domainObjectService = domainObjectService;
            _apiOptions = apiOptions.Value ?? new ApiOptions();
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
        public virtual ActionResult Get([FromRoute] string storageKey)
        {
            var resp = _domainObjectService.Get(storageKey);
            if (resp.Success)
            {
                if (_apiOptions.ReturnResponseObject)
                    return Ok(resp);
                else
                    return Ok(resp.Item);
            }
            return GetErrorResponse(resp);
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
        public virtual ActionResult GetFromQuery([FromQuery] string storageKey)
        {
            var resp = _domainObjectService.Get(storageKey);
            if (resp.Success)
            {
                if (_apiOptions.ReturnResponseObject)
                    return Ok(resp);
                else
                    return Ok(resp.Item);
            }
            return GetErrorResponse(resp);
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
        public virtual async Task<ActionResult> GetAsync([FromRoute] string storageKey)
        {
            var resp = await _domainObjectService.GetAsync(storageKey);
            if (resp.Success)
            {
                if (_apiOptions.ReturnResponseObject)
                    return Ok(resp);
                else
                    return Ok(resp.Item);
            }
            return GetErrorResponse(resp);
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
        public virtual async Task<ActionResult> GetFromQueryAsync([FromQuery] string storageKey)
        {
            var resp = await _domainObjectService.GetAsync(storageKey);
            if (resp.Success)
            {
                if (_apiOptions.ReturnResponseObject)
                    return Ok(resp);
                else
                    return Ok(resp.Item);
            }
            return GetErrorResponse(resp);
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
        public virtual ActionResult Update([FromBody] TDto dto)
        {
            var resp = _domainObjectService.Update(dto);
            if (resp.Success)
            {
                if (_apiOptions.ReturnResponseObject)
                    return Ok(resp);
                else
                    return Ok(resp.Item);
            }
            return GetErrorResponse(resp);
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
        public virtual async Task<ActionResult> UpdateAsync([FromBody] TDto dto)
        {
            var resp = await _domainObjectService.UpdateAsync(dto);
            if (resp.Success)
            {
                if (_apiOptions.ReturnResponseObject)
                    return Ok(resp);
                else
                    return Ok(resp.Item);
            }
            return GetErrorResponse(resp);
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
        public virtual ActionResult Create([FromBody] TDto dto)
        {
            var resp = _domainObjectService.Create(dto);
            if (resp.Success)
            {
                if (_apiOptions.ReturnResponseObject)
                    return Ok(resp);
                else
                    return Ok(resp.Item);
            }
            return GetErrorResponse(resp);
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
        public virtual async Task<ActionResult> CreateAsync([FromBody] TDto dto)
        {
            var resp = await _domainObjectService.CreateAsync(dto);
            if (resp.Success)
            {
                if (_apiOptions.ReturnResponseObject)
                    return Ok(resp);
                else
                    return Ok(resp.Item);
            }
            return GetErrorResponse(resp);
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
        public virtual ActionResult Delete([FromRoute] string storageKey)
        {
            var resp = _domainObjectService.Delete(storageKey);
            if (resp.Success)
            {
                if (_apiOptions.ReturnResponseObject)
                    return Ok(resp);
                else
                    return Ok(true);
            }
            return GetErrorResponse(resp);
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
        public virtual ActionResult DeleteFromQuery([FromQuery] string storageKey)
        {
            var resp = _domainObjectService.Delete(storageKey);
            if (resp.Success)
            {
                if (_apiOptions.ReturnResponseObject)
                    return Ok(resp);
                else
                    return Ok(true);
            }
            return GetErrorResponse(resp);
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
        public virtual async Task<ActionResult> DeleteAsync([FromRoute] string storageKey)
        {
            var resp = await _domainObjectService.DeleteAsync(storageKey);
            if (resp.Success)
            {
                if (_apiOptions.ReturnResponseObject)
                    return Ok(resp);
                else
                    return Ok(true);
            }
            return GetErrorResponse(resp);
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
        public virtual async Task<ActionResult> DeleteFromQueryAsync([FromQuery] string storageKey)
        {
            var resp = await _domainObjectService.DeleteAsync(storageKey);
            if (resp.Success)
            {
                if (_apiOptions.ReturnResponseObject)
                    return Ok(resp);
                else
                    return Ok(true);
            }
            return GetErrorResponse(resp);
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
        public virtual ActionResult Query([FromBody] ServiceQueryRequest request)
        {
            var resp = _domainObjectService.Query(request);
            if (resp.Success)
            {
                if (_apiOptions.ReturnResponseObject)
                    return Ok(resp);
                else
                    return Ok(resp.Item);
            }
            return GetErrorResponse(resp);
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
        public virtual async Task<ActionResult> QueryAsync([FromBody] ServiceQueryRequest request)
        {
            var resp = await _domainObjectService.QueryAsync(request);
            if (resp.Success)
            {
                if (_apiOptions.ReturnResponseObject)
                    return Ok(resp);
                else
                    return Ok(resp.Item);
            }
            return GetErrorResponse(resp);
        }

        /// <summary>
        /// Get the error response
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        [NonAction]
        public virtual ActionResult GetErrorResponse(IResponse response)
        {
            // Create filtered list of messages (Don't expose sensitive system errors)
            List<IResponseMessage> messages = response.Messages.ToList();
            if (!_apiOptions.ExposeSystemErrors)
                messages = messages.Where(x => x.Severity != ResponseSeverity.ErrorSystemSensitive).ToList();
            if (messages.Count == 0)
                messages.Add(ResponseMessage.CreateError(LocalizationResource.ERROR_SYSTEM));

            // Create new error response
            var respError = new Response() { Error = true };
            foreach (var item in messages)
                respError.AddMessage(item);

            // Return response
            if (_apiOptions.ReturnResponseObject)
                return BadRequest(respError);
            else
            {
                string detail = string.Join(Environment.NewLine, messages.Select(x => x.Message));
                var details = new ProblemDetails()
                {
                    Title = LocalizationResource.ERROR_SYSTEM,
                    Status = (int)HttpStatusCode.BadRequest,
                    Detail = detail
                };
                return BadRequest(details);
            }
        }
    }
}