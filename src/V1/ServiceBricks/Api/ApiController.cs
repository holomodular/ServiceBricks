using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using ServiceQuery;
using System.Net;

namespace ServiceBricks
{
    /// <summary>
    /// This is a REST API controller for a domain object.
    /// By default, no security policy is added to this base class.
    /// Use AdminPolicyRestApiController instead.
    /// </summary>
    /// <typeparam name="TDtoObject"></typeparam>
    public partial class ApiController<TDtoObject> : ControllerBase, IApiController<TDtoObject>
        where TDtoObject : class
    {
        protected readonly IApiService<TDtoObject> _domainObjectService;
        protected readonly ApiOptions _apiOptions;

        public ApiController(
            IApiService<TDtoObject> domainObjectService,
            IOptions<ApiOptions> apiOptions)
        {
            _domainObjectService = domainObjectService;
            _apiOptions = apiOptions.Value ?? new ApiOptions();
        }

        [HttpGet]
        [Route("Get")]
        [Route("Get/{storageKey}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public virtual ActionResult Get([FromQuery] string storageKey)
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

        [HttpGet]
        [Route("GetAsync")]
        [Route("GetAsync/{storageKey}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public virtual async Task<ActionResult> GetAsync([FromQuery] string storageKey)
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

        [HttpPut]
        [Route("Update")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public virtual ActionResult Update([FromBody] TDtoObject dto)
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

        [HttpPut]
        [Route("UpdateAsync")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public virtual async Task<ActionResult> UpdateAsync([FromBody] TDtoObject dto)
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

        [HttpPost]
        [Route("Create")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public virtual ActionResult Create([FromBody] TDtoObject dto)
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

        [HttpPost]
        [Route("CreateAsync")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public virtual async Task<ActionResult> CreateAsync([FromBody] TDtoObject dto)
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

        [HttpDelete]
        [Route("Delete")]
        [Route("Delete/{storageKey}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public virtual ActionResult Delete([FromQuery] string storageKey)
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

        [HttpDelete]
        [Route("DeleteAsync")]
        [Route("DeleteAsync/{storageKey}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public virtual async Task<ActionResult> DeleteAsync([FromQuery] string storageKey)
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

        [NonAction]
        public virtual ActionResult GetErrorResponse(IResponse response)
        {
            List<IResponseMessage> messages = response.Messages.ToList();
            if (!_apiOptions.ExposeSystemErrors)
                messages = messages.Where(x => x.Severity != ResponseSeverity.ErrorSystemSensitive).ToList();
            if (messages.Count == 0)
                messages.Add(ResponseMessage.CreateError(LocalizationResource.ERROR_SYSTEM));

            if (_apiOptions.ReturnResponseObject)
            {
                var resp = new Response() { Error = true };
                foreach (var item in messages)
                    resp.AddMessage(item);
                return BadRequest(resp);
            }
            else
            {
                string message = string.Join(Environment.NewLine, messages);
                if (string.IsNullOrEmpty(message))
                    message = LocalizationResource.ERROR_SYSTEM;

                var details = new ProblemDetails()
                {
                    Title = LocalizationResource.ERROR_SYSTEM,
                    Status = (int)HttpStatusCode.BadRequest,
                    Detail = message
                };
                return BadRequest(details);
            }
        }
    }
}