using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.JsonPatch;
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
    public partial class ApiController<TDto> : ApiControllerBase, IApiController<TDto>
        where TDto : class, IDataTransferObject, new()
    {
        protected readonly IApiService<TDto> _domainObjectService;

        public ApiController(
            IApiService<TDto> domainObjectService,
            IOptions<ApiOptions> apiOptions)
            : base(apiOptions)
        {
            _domainObjectService = domainObjectService;
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
            if(_apiOptions.DisableSyncMethods)
            {
                var respErr = new Response();
                respErr.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_API_SYNC_DISABLED));
                return GetErrorResponse(respErr);
            }
            var resp = _domainObjectService.Get(storageKey);
            if (resp.Success)
            {
                if (UseModernResponse())
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
        [Route("Get")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public virtual ActionResult GetFromQuery([FromQuery] string storageKey)
        {
            if (_apiOptions.DisableSyncMethods)
            {
                var respErr = new Response();
                respErr.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_API_SYNC_DISABLED));
                return GetErrorResponse(respErr);
            }
            var resp = _domainObjectService.Get(storageKey);
            if (resp.Success)
            {
                if (UseModernResponse())
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
        [Route("GetAsync/{storageKey}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public virtual async Task<ActionResult> GetAsync(
            [FromRoute] string storageKey,
            CancellationToken cancellationToken = default)
        {
            var resp = await _domainObjectService.GetAsync(storageKey);
            if (resp.Success)
            {
                if (UseModernResponse())
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
        public virtual async Task<ActionResult> GetFromQueryAsync(
            [FromQuery] string storageKey,
            CancellationToken cancellationToken = default)
        {
            var resp = await _domainObjectService.GetAsync(storageKey);
            if (resp.Success)
            {
                if (UseModernResponse())
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
        [Route("Update")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public virtual ActionResult Update([FromBody] TDto dto)
        {
            if (_apiOptions.DisableSyncMethods)
            {
                var respErr = new Response();
                respErr.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_API_SYNC_DISABLED));
                return GetErrorResponse(respErr);
            }
            var resp = _domainObjectService.Update(dto);
            if (resp.Success)
            {
                if (UseModernResponse())
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
        [Route("UpdateAsync")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public virtual async Task<ActionResult> UpdateAsync(
            [FromBody] TDto dto,
            CancellationToken cancellationToken = default)
        {
            var resp = await _domainObjectService.UpdateAsync(dto);
            if (resp.Success)
            {
                if (UseModernResponse())
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
        [Route("UpdateAck")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public virtual ActionResult UpdateAck([FromBody] TDto dto)
        {
            if (_apiOptions.DisableSyncMethods)
            {
                var respErr = new Response();
                respErr.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_API_SYNC_DISABLED));
                return GetErrorResponse(respErr);
            }
            var resp = _domainObjectService.UpdateAck(dto);
            if (resp.Success)
            {
                if (UseModernResponse())
                    return Ok(resp);
                else
                    return Ok(true);
            }
            return GetErrorResponse(resp);
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("UpdateAckAsync")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public virtual async Task<ActionResult> UpdateAckAsync(
            [FromBody] TDto dto,
            CancellationToken cancellationToken = default)
        {
            var resp = await _domainObjectService.UpdateAckAsync(dto);
            if (resp.Success)
            {
                if (UseModernResponse())
                    return Ok(resp);
                else
                    return Ok(true);
            }
            return GetErrorResponse(resp);
        }

        /// <summary>
        /// Create
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]        
        [Route("Create")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public virtual ActionResult Create([FromBody] TDto dto)
        {
            if (_apiOptions.DisableSyncMethods)
            {
                var respErr = new Response();
                respErr.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_API_SYNC_DISABLED));
                return GetErrorResponse(respErr);
            }
            var resp = _domainObjectService.Create(dto);
            if (resp.Error)
                return GetErrorResponse(resp);

            if (Response != null && resp.Item != null && !string.IsNullOrWhiteSpace(resp.Item.StorageKey))
            {
                var encodedKey = Uri.EscapeDataString(resp.Item.StorageKey);                
                Response.Headers.Location = $"Get?storageKey={encodedKey}";
            }            
            if (UseModernResponse())
                return StatusCode((int)HttpStatusCode.Created, resp);
            else
                return StatusCode((int)HttpStatusCode.Created, resp.Item);            
        }

        /// <summary>
        /// Create
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        [Route("CreateAsync")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public virtual async Task<ActionResult> CreateAsync(
            [FromBody] TDto dto,
            CancellationToken cancellationToken = default)
        {
            var resp = await _domainObjectService.CreateAsync(dto);
            if (resp.Error)
                return GetErrorResponse(resp);

            if (Response != null && resp.Item != null && !string.IsNullOrWhiteSpace(resp.Item.StorageKey))
            {
                var encodedKey = Uri.EscapeDataString(resp.Item.StorageKey);
                Response.Headers.Location = $"GetAsync?storageKey={encodedKey}";
            }
            if (UseModernResponse())
                return StatusCode((int)HttpStatusCode.Created, resp);
            else
                return StatusCode((int)HttpStatusCode.Created, resp.Item);            
        }

        /// <summary>
        /// Create
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CreateAck")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public virtual ActionResult CreateAck([FromBody] TDto dto)
        {
            if (_apiOptions.DisableSyncMethods)
            {
                var respErr = new Response();
                respErr.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_API_SYNC_DISABLED));
                return GetErrorResponse(respErr);
            }
            var resp = _domainObjectService.Create(dto);
            if (resp.Error)
                return GetErrorResponse(resp);

            if (Response != null && resp.Item != null && !string.IsNullOrEmpty(resp.Item.StorageKey))
            {
                var encodedKey = Uri.EscapeDataString(resp.Item.StorageKey);
                Response.Headers.Location = $"Get?storageKey={encodedKey}";
            }

            if (UseModernResponse())
                return StatusCode((int)HttpStatusCode.Created, resp);
            else
                return StatusCode((int)HttpStatusCode.Created, true);            
        }

        /// <summary>
        /// Create
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CreateAckAsync")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public virtual async Task<ActionResult> CreateAckAsync(
            [FromBody] TDto dto,
            CancellationToken cancellationToken = default)
        {
            var resp = await _domainObjectService.CreateAsync(dto);
            if (resp.Error)
                return GetErrorResponse(resp);

            if (Response != null && resp.Item != null && !string.IsNullOrEmpty(resp.Item.StorageKey))
            {
                var encodedKey = Uri.EscapeDataString(resp.Item.StorageKey);
                Response.Headers.Location = $"GetAsync?storageKey={encodedKey}";
            }

            if (UseModernResponse())
                return StatusCode((int)HttpStatusCode.Created, resp);
            else
                return StatusCode((int)HttpStatusCode.Created, true);

        }


        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="storageKey"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{storageKey}")]
        [Route("Delete/{storageKey}")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public virtual ActionResult Delete([FromRoute] string storageKey)
        {
            if (_apiOptions.DisableSyncMethods)
            {
                var respErr = new Response();
                respErr.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_API_SYNC_DISABLED));
                return GetErrorResponse(respErr);
            }
            var resp = _domainObjectService.Delete(storageKey);
            if (resp.Success)
            {
                if (UseModernResponse())
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
        [Route("Delete")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public virtual ActionResult DeleteFromQuery([FromQuery] string storageKey)
        {
            if (_apiOptions.DisableSyncMethods)
            {
                var respErr = new Response();
                respErr.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_API_SYNC_DISABLED));
                return GetErrorResponse(respErr);
            }
            var resp = _domainObjectService.Delete(storageKey);
            if (resp.Success)
            {
                if (UseModernResponse())
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
        [Route("DeleteAsync/{storageKey}")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public virtual async Task<ActionResult> DeleteAsync(
            [FromRoute] string storageKey,
            CancellationToken cancellationToken = default)
        {
            var resp = await _domainObjectService.DeleteAsync(storageKey);
            if (resp.Success)
            {
                if (UseModernResponse())
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
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public virtual async Task<ActionResult> DeleteFromQueryAsync(
            [FromQuery] string storageKey,
            CancellationToken cancellationToken = default)
        {
            var resp = await _domainObjectService.DeleteAsync(storageKey);
            if (resp.Success)
            {
                if (UseModernResponse())
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
            if (_apiOptions.DisableSyncMethods)
            {
                var respErr = new Response();
                respErr.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_API_SYNC_DISABLED));
                return GetErrorResponse(respErr);
            }
            var resp = _domainObjectService.Query(request);
            if (resp.Success)
            {
                if (UseModernResponse())
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
        public virtual async Task<ActionResult> QueryAsync(
            [FromBody] ServiceQueryRequest request,
            CancellationToken cancellationToken = default)
        {
            var resp = await _domainObjectService.QueryAsync(request);
            if (resp.Success)
            {
                if (UseModernResponse())
                    return Ok(resp);
                else
                    return Ok(resp.Item);
            }
            return GetErrorResponse(resp);
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
        public virtual ActionResult Patch(
            [FromRoute] string storageKey,
            [FromBody] JsonPatchDocument<TDto> patchDocument)
        {
            if (_apiOptions.DisableSyncMethods)
            {
                var respErr = new Response();
                respErr.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_API_SYNC_DISABLED));
                return GetErrorResponse(respErr);
            }
            var resp = _domainObjectService.Patch(storageKey, patchDocument);
            if (resp.Success)
            {
                if (UseModernResponse())
                    return Ok(resp);
                else
                    return Ok(resp.Item);
            }
            return GetErrorResponse(resp);
        }

        /// <summary>
        /// Patch
        /// </summary>
        /// <param name="storageKey"></param>
        /// <param name="patchDocument"></param>
        /// <returns></returns>
        [HttpPatch]        
        [Route("Patch")]
        [Consumes("application/json-patch+json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public virtual ActionResult PatchFromQuery(
            [FromQuery] string storageKey,
            [FromBody] JsonPatchDocument<TDto> patchDocument)
        {
            if (_apiOptions.DisableSyncMethods)
            {
                var respErr = new Response();
                respErr.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_API_SYNC_DISABLED));
                return GetErrorResponse(respErr);
            }
            var resp = _domainObjectService.Patch(storageKey, patchDocument);
            if (resp.Success)
            {
                if (UseModernResponse())
                    return Ok(resp);
                else
                    return Ok(resp.Item);
            }
            return GetErrorResponse(resp);
        }


        /// <summary>
        /// Patch
        /// </summary>
        /// <param name="storageKey"></param>
        /// <param name="patchDocument"></param>
        /// <returns></returns>
        [HttpPatch]
        [Route("")]
        [Route("PatchAsync/{storageKey}")]
        [Consumes("application/json-patch+json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public virtual async Task<ActionResult> PatchAsync(
            [FromRoute] string storageKey,
            [FromBody] JsonPatchDocument<TDto> patchDocument,
            CancellationToken cancellationToken = default)
        {
            var resp = await _domainObjectService.PatchAsync(storageKey, patchDocument);
            if (resp.Success)
            {
                if (UseModernResponse())
                    return Ok(resp);
                else
                    return Ok(resp.Item);
            }
            return GetErrorResponse(resp);
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
        public virtual async Task<ActionResult> PatchFromQueryAsync(
            [FromQuery] string storageKey,
            [FromBody] JsonPatchDocument<TDto> patchDocument,
            CancellationToken cancellationToken = default)
        {
            var resp = await _domainObjectService.PatchAsync(storageKey, patchDocument);
            if (resp.Success)
            {
                if (UseModernResponse())
                    return Ok(resp);
                else
                    return Ok(resp.Item);
            }
            return GetErrorResponse(resp);
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
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public virtual ActionResult PatchAck(
            [FromRoute] string storageKey,
            [FromBody] JsonPatchDocument<TDto> patchDocument)
        {
            if (_apiOptions.DisableSyncMethods)
            {
                var respErr = new Response();
                respErr.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_API_SYNC_DISABLED));
                return GetErrorResponse(respErr);
            }
            var resp = _domainObjectService.PatchAck(storageKey, patchDocument);
            if (resp.Success)
            {
                if (UseModernResponse())
                    return Ok(resp);
                else
                    return Ok(true);
            }
            return GetErrorResponse(resp);
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
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public virtual ActionResult PatchAckFromQuery(
            [FromQuery] string storageKey,
            [FromBody] JsonPatchDocument<TDto> patchDocument)
        {
            if (_apiOptions.DisableSyncMethods)
            {
                var respErr = new Response();
                respErr.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_API_SYNC_DISABLED));
                return GetErrorResponse(respErr);
            }
            var resp = _domainObjectService.PatchAck(storageKey, patchDocument);
            if (resp.Success)
            {
                if (UseModernResponse())
                    return Ok(resp);
                else
                    return Ok(true);
            }
            return GetErrorResponse(resp);
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
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public virtual async Task<ActionResult> PatchAckAsync(
            [FromRoute] string storageKey,
            [FromBody] JsonPatchDocument<TDto> patchDocument,
            CancellationToken cancellationToken = default)
        {
            var resp = await _domainObjectService.PatchAckAsync(storageKey, patchDocument);
            if (resp.Success)
            {
                if (UseModernResponse())
                    return Ok(resp);
                else
                    return Ok(true);
            }
            return GetErrorResponse(resp);
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
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public virtual async Task<ActionResult> PatchAckFromQueryAsync(
            [FromQuery] string storageKey,
            [FromBody] JsonPatchDocument<TDto> patchDocument,
            CancellationToken cancellationToken = default)
        {
            var resp = await _domainObjectService.PatchAckAsync(storageKey, patchDocument);
            if (resp.Success)
            {
                if (UseModernResponse())
                    return Ok(resp);
                else
                    return Ok(true);
            }
            return GetErrorResponse(resp);
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
        public virtual ActionResult Validate([FromBody] TDto dto)
        {
            if (_apiOptions.DisableSyncMethods)
            {
                var respErr = new Response();
                respErr.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_API_SYNC_DISABLED));
                return GetErrorResponse(respErr);
            }
            var resp = _domainObjectService.Validate(dto);
            if (resp.Error)
                return GetErrorResponse(resp);
            
            if (UseModernResponse())
                return StatusCode((int)HttpStatusCode.OK, resp);
            else
                return StatusCode((int)HttpStatusCode.OK, true);
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
        public virtual async Task<ActionResult> ValidateAsync([FromBody] TDto dto, CancellationToken cancellationToken = default)
        {
            var resp = await _domainObjectService.ValidateAsync(dto);
            if (resp.Error)
                return GetErrorResponse(resp);

            if (UseModernResponse())
                return StatusCode((int)HttpStatusCode.OK, resp);
            else
                return StatusCode((int)HttpStatusCode.OK, true);
        }

    }
}