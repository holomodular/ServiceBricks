using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServiceQuery;
using System.Text;
using Microsoft.AspNetCore.JsonPatch;

namespace ServiceBricks
{
    /// <summary>
    /// This is a REST API service client for a DTO.
    /// </summary>
    /// <typeparam name="TDto"></typeparam>
    public partial class ApiClient<TDto> : BearerTokenClient, IApiClient<TDto>
        where TDto : class, IDataTransferObject
    {
        protected readonly ClientApiOptions _clientApiOptions;
        protected readonly ILogger<ApiClient<TDto>> _logger;
        protected Type _type = null;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="loggerFactory"></param>
        /// <param name="httpClientFactory"></param>
        /// <param name="clientApiOptions"></param>
        public ApiClient(
            ILoggerFactory loggerFactory,
            IHttpClientFactory httpClientFactory,
            ClientApiOptions clientApiOptions) // Don't use IOptions here so that it can be changed in derived classes
            : base(loggerFactory, httpClientFactory, clientApiOptions.DisableAuthentication ? null : new BearerTokenCredentials()
            {
                AccessTokenRequest = new AccessTokenRequest()
                {
                    client_id = clientApiOptions.TokenClient,
                    client_secret = clientApiOptions.TokenSecret,
                    grant_type = clientApiOptions.TokenType,
                    response_type = clientApiOptions.TokenResponseType,
                    scope = clientApiOptions.TokenScope,
                },
                AuthorizationUrl = clientApiOptions.TokenUrl,
            })
        {
            _logger = loggerFactory.CreateLogger<ApiClient<TDto>>();
            _clientApiOptions = clientApiOptions;
            _type = typeof(TDto);
            BaseUrl = _clientApiOptions.BaseServiceUrl;
            if (_clientApiOptions.ReturnResponseObject)
                ApiResponseFormat = ApiResponseFormat.modern;
            else
                ApiResponseFormat = ApiResponseFormat.classic;
        }

        /// <summary>
        /// Constructor for manually passing in an access token.
        /// </summary>
        /// <param name="loggerFactory"></param>
        /// <param name="httpClientFactory"></param>
        /// <param name="clientApiOptions"></param>
        /// <param name="existingToken"></param>
        public ApiClient(
            ILoggerFactory loggerFactory,
            IHttpClientFactory httpClientFactory,
            ClientApiOptions clientApiOptions,
            AccessTokenResponse existingToken)
    : base(loggerFactory, httpClientFactory,
        clientApiOptions.DisableAuthentication ? null : new BearerTokenCredentials()
        {
            AccessTokenRequest = new AccessTokenRequest()
            {
                client_id = clientApiOptions.TokenClient,
                client_secret = clientApiOptions.TokenSecret,
                grant_type = clientApiOptions.TokenType,
                response_type = clientApiOptions.TokenResponseType,
                scope = clientApiOptions.TokenScope,
            },
            AuthorizationUrl = clientApiOptions.TokenUrl,
            AccessTokenResponse = existingToken
        })
        {
            _logger = loggerFactory.CreateLogger<ApiClient<TDto>>();
            _clientApiOptions = clientApiOptions;
            _type = typeof(TDto);
            BaseUrl = _clientApiOptions.BaseServiceUrl;
            if (_clientApiOptions.ReturnResponseObject)
                ApiResponseFormat = ApiResponseFormat.modern;
            else
                ApiResponseFormat = ApiResponseFormat.classic;
        }

        /// <summary>
        /// This the base url of the API request.
        /// </summary>
        public virtual string BaseUrl { get; set; }

        /// <summary>
        /// This is the resource for the API request.
        /// </summary>
        public virtual string ApiResource { get; set; }

        /// <summary>
        /// This is the resource for the API request.
        /// </summary>
        public virtual ApiResponseFormat ApiResponseFormat { get; set; }



        /// <summary>
        /// Get the response format
        /// </summary>
        /// <returns></returns>
        protected virtual string GetResponseFormat()
        {
            switch(ApiResponseFormat)
            {
                default:
                case ApiResponseFormat.modern:
                    return "modern";

                case ApiResponseFormat.classic:
                    return "classic";                
            }
        }


        /// <summary>
        /// Execute the request.
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        public virtual IResponseItem<TModel> Execute<TModel>(HttpRequestMessage request)
        {
            // Validation
            var resp = new ResponseItem<TModel>();
            if (request == null)
            {
                resp.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, nameof(request)));
                return resp;
            }

            try
            {
                // Send and get response
                var result = Send(request);
                if (result == null)
                {
                    _logger.LogError("ApiClient SendAsync returned null");
                    resp.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_REST_CLIENT));
                    return resp;
                }

                // Get content
                string content = null;
                if (result.Content != null)
                {
                    using (var stream = result.Content.ReadAsStream())
                    using (StreamReader reader = new StreamReader(stream))
                        content = reader.ReadToEnd();
                }

                // Check response status
                if (result.IsSuccessStatusCode)
                {
                    // Deserialize content to ITEM in the format of TModel
                    if (!string.IsNullOrEmpty(content))
                        resp.Item = JsonSerializer.Instance.DeserializeObject<TModel>(content);
                }
                else
                {
                    // Mark message as error
                    resp.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_REST_CLIENT));

                    // Check response format
                    if (_clientApiOptions.ReturnResponseObject)
                    {
                        if (!string.IsNullOrEmpty(content))
                            resp.Item = JsonSerializer.Instance.DeserializeObject<TModel>(content);
                    }
                    else
                    {
                        try
                        {
                            var pd = JsonSerializer.Instance.DeserializeObject<ProblemDetails>(content);
                            if (pd != null)
                                resp.AddMessage(ResponseMessage.CreateError(pd.Title + ":" + pd.Detail));
                        }
                        catch { } // Other API use possible that don't use ProblemDetails
                    }
                }
                return resp;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                resp.AddMessage(ResponseMessage.CreateError(ex, LocalizationResource.ERROR_REST_CLIENT));
                return resp;
            }
        }

        /// <summary>
        /// Execute the request async.
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        public virtual async Task<IResponseItem<TModel>> ExecuteAsync<TModel>(HttpRequestMessage request)
        {
            // Validation
            var resp = new ResponseItem<TModel>();
            if (request == null)
            {
                resp.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, nameof(request)));
                return resp;
            }

            try
            {
                // Send and get response
                var result = await SendAsync(request);
                if (result == null)
                {
                    _logger.LogError("ApiClient SendAsync returned null");
                    resp.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_REST_CLIENT));
                    return resp;
                }

                // Get content
                string content = null;
                if (result.Content != null)
                    content = await result.Content.ReadAsStringAsync();

                // Check response status
                if (result.IsSuccessStatusCode)
                {
                    // Deserialize content to ITEM in the format of TModel
                    if (!string.IsNullOrEmpty(content))
                        resp.Item = JsonSerializer.Instance.DeserializeObject<TModel>(content);
                    return resp;
                }
                else
                {
                    // Mark message as error
                    resp.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_REST_CLIENT));

                    // Check response format
                    if (_clientApiOptions.ReturnResponseObject)
                    {
                        if (!string.IsNullOrEmpty(content))
                            resp.Item = JsonSerializer.Instance.DeserializeObject<TModel>(content);
                    }
                    else
                    {
                        try
                        {
                            var pd = JsonSerializer.Instance.DeserializeObject<ProblemDetails>(content);
                            if (pd != null)
                                resp.AddMessage(ResponseMessage.CreateError(pd.Title + ":" + pd.Detail));
                        }
                        catch { } // Other API use possible that don't use ProblemDetails
                    }
                    return resp;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                resp.AddMessage(ResponseMessage.CreateError(ex, LocalizationResource.ERROR_REST_CLIENT));
                return resp;
            }
        }

        /// <summary>
        /// Get a domain object by its storagekey.
        /// </summary>
        /// <param name="storageKey"></param>
        /// <returns></returns>
        public virtual IResponseItem<TDto> Get(string storageKey)
        {
            // Create request
            var encodedStorageKey = Uri.EscapeDataString(storageKey ?? string.Empty);
            HttpRequestMessage request = new HttpRequestMessage(
                HttpMethod.Get,
                $"{BaseUrl}/{ApiResource}/Get?format={GetResponseFormat()}&storageKey={encodedStorageKey}");

            // Execute request
            if (ApiResponseFormat == ApiResponseFormat.modern)
            {
                var resp = Execute<ResponseItem<TDto>>(request);
                if (resp.Success && resp.Item != null)
                    return resp.Item;

                ResponseItem<TDto> respEr = new ResponseItem<TDto>();
                respEr.CopyFrom(resp);
                return respEr;
            }
            else
                return Execute<TDto>(request);
        }

        /// <summary>
        /// Get a domain object by its storagekey.
        /// </summary>
        /// <param name="storageKey"></param>
        /// <returns></returns>
        public virtual async Task<IResponseItem<TDto>> GetAsync(string storageKey)
        {
            // Create request
            var encodedStorageKey = Uri.EscapeDataString(storageKey ?? string.Empty);
            HttpRequestMessage request = new HttpRequestMessage(
                HttpMethod.Get,
                $"{BaseUrl}/{ApiResource}/GetAsync?format={GetResponseFormat()}&storageKey={encodedStorageKey}");

            // Execute request
            if (ApiResponseFormat == ApiResponseFormat.modern)
            {
                var resp = await ExecuteAsync<ResponseItem<TDto>>(request);
                if (resp.Success && resp.Item != null)
                    return resp.Item;

                ResponseItem<TDto> respEr = new ResponseItem<TDto>();
                respEr.CopyFrom(resp);
                return respEr;
            }
            else
                return await ExecuteAsync<TDto>(request);
        }

        /// <summary>
        /// Update a domain object.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public virtual IResponseItem<TDto> Update(TDto dto)
        {
            // Validation
            if (dto == null)
            {
                var errresp = new ResponseItem<TDto>();
                errresp.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, nameof(dto)));
                return errresp;
            }

            // Create request
            HttpRequestMessage request = new HttpRequestMessage(
                HttpMethod.Put,
                $"{BaseUrl}/{ApiResource}/Update?format={GetResponseFormat()}");
            string data = JsonSerializer.Instance.SerializeObject(dto);
            request.Content = new StringContent(data, Encoding.UTF8, CONTENTTYPE_APPLICATIONJSON);

            // Execute request
            if (ApiResponseFormat == ApiResponseFormat.modern)
            {
                var resp = Execute<ResponseItem<TDto>>(request);
                if (resp.Success && resp.Item != null)
                    return resp.Item;

                ResponseItem<TDto> respEr = new ResponseItem<TDto>();
                respEr.CopyFrom(resp);
                return respEr;
            }
            else
                return Execute<TDto>(request);
        }

        /// <summary>
        /// Update a domain object.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public virtual async Task<IResponseItem<TDto>> UpdateAsync(TDto dto)
        {
            // Validation
            if (dto == null)
            {
                var errresp = new ResponseItem<TDto>();
                errresp.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, nameof(dto)));
                return errresp;
            }

            // Create request
            HttpRequestMessage request = new HttpRequestMessage(
                HttpMethod.Put,
                $"{BaseUrl}/{ApiResource}/UpdateAsync?format={GetResponseFormat()}");
            string data = JsonSerializer.Instance.SerializeObject(dto);
            request.Content = new StringContent(data, Encoding.UTF8, CONTENTTYPE_APPLICATIONJSON);

            // Execute request
            if (ApiResponseFormat == ApiResponseFormat.modern)
            {
                var resp = await ExecuteAsync<ResponseItem<TDto>>(request);
                if (resp.Success && resp.Item != null)
                    return resp.Item;

                ResponseItem<TDto> respEr = new ResponseItem<TDto>();
                respEr.CopyFrom(resp);
                return respEr;
            }
            else
                return await ExecuteAsync<TDto>(request);
        }

        /// <summary>
        /// Update a domain object.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public virtual IResponse UpdateAck(TDto dto)
        {
            // Validation
            if (dto == null)
            {
                var errresp = new Response();
                errresp.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, nameof(dto)));
                return errresp;
            }

            // Create request
            HttpRequestMessage request = new HttpRequestMessage(
                HttpMethod.Put,
                $"{BaseUrl}/{ApiResource}/UpdateAck?format={GetResponseFormat()}");
            string data = JsonSerializer.Instance.SerializeObject(dto);
            request.Content = new StringContent(data, Encoding.UTF8, CONTENTTYPE_APPLICATIONJSON);

            // Execute request
            if (ApiResponseFormat == ApiResponseFormat.modern)
                return Execute<Response>(request);            
            else
                return Execute<bool>(request);
        }

        /// <summary>
        /// Update a domain object.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public virtual async Task<IResponse> UpdateAckAsync(TDto dto)
        {
            // Validation
            if (dto == null)
            {
                var errresp = new Response();
                errresp.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, nameof(dto)));
                return errresp;
            }

            // Create request
            HttpRequestMessage request = new HttpRequestMessage(
                HttpMethod.Put,
                $"{BaseUrl}/{ApiResource}/UpdateAckAsync?format={GetResponseFormat()}");
            string data = JsonSerializer.Instance.SerializeObject(dto);
            request.Content = new StringContent(data, Encoding.UTF8, CONTENTTYPE_APPLICATIONJSON);

            // Execute request
            if (ApiResponseFormat == ApiResponseFormat.modern)
                return await ExecuteAsync<Response>(request);                
            else
                return await ExecuteAsync<bool>(request);
        }

        /// <summary>
        /// Create a domain object.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public virtual IResponseItem<TDto> Create(TDto dto)
        {
            // Validation
            if (dto == null)
            {
                var errresp = new ResponseItem<TDto>();
                errresp.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, nameof(dto)));
                return errresp;
            }

            // Create request
            HttpRequestMessage request = new HttpRequestMessage(
                HttpMethod.Post,
                $"{BaseUrl}/{ApiResource}/Create?format={GetResponseFormat()}");
            string data = JsonSerializer.Instance.SerializeObject(dto);
            request.Content = new StringContent(data, Encoding.UTF8, CONTENTTYPE_APPLICATIONJSON);

            // Execute request
            if (ApiResponseFormat == ApiResponseFormat.modern)
            {
                var resp = Execute<ResponseItem<TDto>>(request);
                if (resp.Success && resp.Item != null)
                    return resp.Item;

                ResponseItem<TDto> respEr = new ResponseItem<TDto>();
                respEr.CopyFrom(resp);
                return respEr;
            }
            else
                return Execute<TDto>(request);
        }

        /// <summary>
        /// Create a domain object.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public virtual async Task<IResponseItem<TDto>> CreateAsync(TDto dto)
        {
            // Validation
            if (dto == null)
            {
                var errresp = new ResponseItem<TDto>();
                errresp.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, nameof(dto)));
                return errresp;
            }

            // Create request
            HttpRequestMessage request = new HttpRequestMessage(
                HttpMethod.Post,
                $"{BaseUrl}/{ApiResource}/CreateAsync?format={GetResponseFormat()}");
            string data = JsonSerializer.Instance.SerializeObject(dto);
            request.Content = new StringContent(data, Encoding.UTF8, CONTENTTYPE_APPLICATIONJSON);

            // Execute request
            if (ApiResponseFormat == ApiResponseFormat.modern)
            {
                var resp = await ExecuteAsync<ResponseItem<TDto>>(request);
                if (resp.Success && resp.Item != null)
                    return resp.Item;

                ResponseItem<TDto> respEr = new ResponseItem<TDto>();
                respEr.CopyFrom(resp);
                return respEr;
            }
            else
                return await ExecuteAsync<TDto>(request);
        }

        /// <summary>
        /// Create a domain object.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public virtual IResponse CreateAck(TDto dto)
        {
            // Validation
            if (dto == null)
            {
                var errresp = new Response();
                errresp.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, nameof(dto)));
                return errresp;
            }

            // Create request
            HttpRequestMessage request = new HttpRequestMessage(
                HttpMethod.Post,
                $"{BaseUrl}/{ApiResource}/CreateAck?format={GetResponseFormat()}");
            string data = JsonSerializer.Instance.SerializeObject(dto);
            request.Content = new StringContent(data, Encoding.UTF8, CONTENTTYPE_APPLICATIONJSON);

            // Execute request
            if (ApiResponseFormat == ApiResponseFormat.modern)
                return Execute<Response>(request);                
            else
                return Execute<bool>(request);
        }

        /// <summary>
        /// Create a domain object.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public virtual async Task<IResponse> CreateAckAsync(TDto dto)
        {
            // Validation
            if (dto == null)
            {
                var errresp = new Response();
                errresp.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, nameof(dto)));
                return errresp;
            }

            // Create request
            HttpRequestMessage request = new HttpRequestMessage(
                HttpMethod.Post,
                $"{BaseUrl}/{ApiResource}/CreateAckAsync?format={GetResponseFormat()}");
            string data = JsonSerializer.Instance.SerializeObject(dto);
            request.Content = new StringContent(data, Encoding.UTF8, CONTENTTYPE_APPLICATIONJSON);

            // Execute request
            if (ApiResponseFormat == ApiResponseFormat.modern)
                return await ExecuteAsync<Response>(request);                
            else
                return await ExecuteAsync<bool>(request);
        }

        /// <summary>
        /// Delete a domain object.
        /// </summary>
        /// <param name="storageKey"></param>
        /// <returns></returns>
        public virtual IResponse Delete(string storageKey)
        {
            // Create request
            HttpRequestMessage request = new HttpRequestMessage(
                HttpMethod.Delete,
                $"{BaseUrl}/{ApiResource}/Delete?format={GetResponseFormat()}&storageKey={storageKey}");

            // Execute request
            if (ApiResponseFormat == ApiResponseFormat.modern)
                return Execute<Response>(request);
            else
                return Execute<bool>(request);
        }

        /// <summary>
        /// Delete a domain object.
        /// </summary>
        /// <param name="storageKey"></param>
        /// <returns></returns>
        public virtual async Task<IResponse> DeleteAsync(string storageKey)
        {
            // Create request
            var encodedStorageKey = Uri.EscapeDataString(storageKey ?? string.Empty);
            HttpRequestMessage request = new HttpRequestMessage(
                HttpMethod.Delete,
                $"{BaseUrl}/{ApiResource}/DeleteAsync?format={GetResponseFormat()}&storageKey={encodedStorageKey}");

            // Execute request
            if (ApiResponseFormat == ApiResponseFormat.modern)
                return await ExecuteAsync<Response>(request);
            else
                return await ExecuteAsync<bool>(request);
        }

        /// <summary>
        /// Query domain objects.
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public virtual IResponseItem<ServiceQueryResponse<TDto>> Query(ServiceQueryRequest serviceQueryRequest)
        {
            // Validation
            if (serviceQueryRequest == null)
            {
                var errresp = new ResponseItem<ServiceQueryResponse<TDto>>();
                errresp.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, nameof(serviceQueryRequest)));
                return errresp;
            }

            // Create request
            HttpRequestMessage request = new HttpRequestMessage(
                HttpMethod.Post,
                $"{BaseUrl}/{ApiResource}/Query?format={GetResponseFormat()}");
            string data = JsonSerializer.Instance.SerializeObject(serviceQueryRequest);
            request.Content = new StringContent(data, Encoding.UTF8, CONTENTTYPE_APPLICATIONJSON);

            // Execute request
            if (ApiResponseFormat == ApiResponseFormat.modern)
            {
                var resp = Execute<ResponseItem<ServiceQueryResponse<TDto>>>(request);
                if (resp.Success && resp.Item != null)
                    return resp.Item;

                ResponseItem<ServiceQueryResponse<TDto>> respEr = new ResponseItem<ServiceQueryResponse<TDto>>();
                respEr.CopyFrom(resp);
                return respEr;
            }
            else
                return Execute<ServiceQueryResponse<TDto>>(request);
        }

        /// <summary>
        /// Query domain objects.
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public virtual async Task<IResponseItem<ServiceQueryResponse<TDto>>> QueryAsync(ServiceQueryRequest serviceQueryRequest)
        {
            // Validation
            if (serviceQueryRequest == null)
            {
                var errresp = new ResponseItem<ServiceQueryResponse<TDto>>();
                errresp.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, nameof(serviceQueryRequest)));
                return errresp;
            }

            // Create request
            HttpRequestMessage request = new HttpRequestMessage(
                HttpMethod.Post,
                $"{BaseUrl}/{ApiResource}/QueryAsync?format={GetResponseFormat()}");
            string data = JsonSerializer.Instance.SerializeObject(serviceQueryRequest);
            request.Content = new StringContent(data, Encoding.UTF8, CONTENTTYPE_APPLICATIONJSON);

            // Execute request
            if (ApiResponseFormat == ApiResponseFormat.modern)
            {
                var resp = await ExecuteAsync<ResponseItem<ServiceQueryResponse<TDto>>>(request);
                if (resp.Success && resp.Item != null)
                    return resp.Item;

                ResponseItem<ServiceQueryResponse<TDto>> respEr = new ResponseItem<ServiceQueryResponse<TDto>>();
                respEr.CopyFrom(resp);
                return respEr;
            }
            else
                return await ExecuteAsync<ServiceQueryResponse<TDto>>(request);
        }

        /// <summary>
        /// Patch
        /// </summary>
        /// <param name="storageKey"></param>
        /// <param name="patchDocument"></param>
        /// <returns></returns>
        public virtual IResponseItem<TDto> Patch(string storageKey, JsonPatchDocument<TDto> patchDocument)
        {
            // Validation
            if (string.IsNullOrEmpty(storageKey))
            {
                var errresp = new ResponseItem<TDto>();
                errresp.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, nameof(storageKey)));
                return errresp;
            }
            if (patchDocument == null)
            {
                var errresp = new ResponseItem<TDto>();
                errresp.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, nameof(patchDocument)));
                return errresp;
            }

            // Create request
            var encodedStorageKey = Uri.EscapeDataString(storageKey ?? string.Empty);
            var request = new HttpRequestMessage(
                HttpMethod.Patch,
                $"{BaseUrl}/{ApiResource}/Patch?format={GetResponseFormat()}&storageKey={encodedStorageKey}");
            
            var data = JsonSerializer.Instance.SerializeObject(patchDocument);
            request.Content = new StringContent(data, Encoding.UTF8, CONTENTTYPE_APPLICATIONJSONPATCH);

            // Execute request
            if (ApiResponseFormat == ApiResponseFormat.modern)
            {
                var resp = Execute<ResponseItem<TDto>>(request);
                if (resp.Success && resp.Item != null)
                    return resp.Item;

                ResponseItem<TDto> respEr = new ResponseItem<TDto>();
                respEr.CopyFrom(resp);
                return respEr;
            }
            else
                return Execute<TDto>(request);
        }

        /// <summary>
        /// Patch
        /// </summary>
        /// <param name="storageKey"></param>
        /// <param name="patchDocument"></param>
        /// <returns></returns>
        public virtual async Task<IResponseItem<TDto>> PatchAsync(string storageKey, JsonPatchDocument<TDto> patchDocument)
        {
            // Validation
            if (string.IsNullOrEmpty(storageKey))
            {
                var errresp = new ResponseItem<TDto>();
                errresp.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, nameof(storageKey)));
                return errresp;
            }

            if (patchDocument == null)
            {
                var errresp = new ResponseItem<TDto>();
                errresp.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, nameof(patchDocument)));
                return errresp;
            }

            // Create request
            var encodedStorageKey = Uri.EscapeDataString(storageKey ?? string.Empty);
            var request = new HttpRequestMessage(
                HttpMethod.Patch,
                $"{BaseUrl}/{ApiResource}/PatchAsync?format={GetResponseFormat()}&storageKey={encodedStorageKey}");

            var data = JsonSerializer.Instance.SerializeObject(patchDocument);
            request.Content = new StringContent(data, Encoding.UTF8, CONTENTTYPE_APPLICATIONJSONPATCH);

            // Execute request
            if (ApiResponseFormat == ApiResponseFormat.modern)
            {
                var resp = await ExecuteAsync<ResponseItem<TDto>>(request);
                if (resp.Success && resp.Item != null)
                    return resp.Item;

                ResponseItem<TDto> respEr = new ResponseItem<TDto>();
                respEr.CopyFrom(resp);
                return respEr;
            }
            else
                return await ExecuteAsync<TDto>(request);
        }


        /// <summary>
        /// PatchAck
        /// </summary>
        /// <param name="storageKey"></param>
        /// <param name="patchDocument"></param>
        /// <returns></returns>
        public virtual IResponse PatchAck(string storageKey, JsonPatchDocument<TDto> patchDocument)
        {
            // Validation
            if (string.IsNullOrEmpty(storageKey))
            {
                var errresp = new Response();
                errresp.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, nameof(storageKey)));
                return errresp;
            }
            if (patchDocument == null)
            {
                var errresp = new Response();
                errresp.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, nameof(patchDocument)));
                return errresp;
            }

            // Create request
            var encodedStorageKey = Uri.EscapeDataString(storageKey ?? string.Empty);
            var request = new HttpRequestMessage(
                HttpMethod.Patch,
                $"{BaseUrl}/{ApiResource}/PatchAck?format={GetResponseFormat()}&storageKey={encodedStorageKey}");

            var data = JsonSerializer.Instance.SerializeObject(patchDocument);
            request.Content = new StringContent(data, Encoding.UTF8, CONTENTTYPE_APPLICATIONJSONPATCH);

            // Execute request
            if (ApiResponseFormat == ApiResponseFormat.modern)
                return Execute<Response>(request);
            else
                return Execute<bool>(request);
        }

        /// <summary>
        /// Patch
        /// </summary>
        /// <param name="storageKey"></param>
        /// <param name="patchDocument"></param>
        /// <returns></returns>
        public virtual async Task<IResponse> PatchAckAsync(string storageKey, JsonPatchDocument<TDto> patchDocument)
        {
            // Validation
            if (string.IsNullOrEmpty(storageKey))
            {
                var errresp = new Response();
                errresp.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, nameof(storageKey)));
                return errresp;
            }
            if (patchDocument == null)
            {
                var errresp = new Response();
                errresp.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, nameof(patchDocument)));
                return errresp;
            }

            // Create request
            var encodedStorageKey = Uri.EscapeDataString(storageKey ?? string.Empty);
            var request = new HttpRequestMessage(
                HttpMethod.Patch,
                $"{BaseUrl}/{ApiResource}/PatchAckAsync?format={GetResponseFormat()}&storageKey={encodedStorageKey}");

            var data = JsonSerializer.Instance.SerializeObject(patchDocument);
            request.Content = new StringContent(data, Encoding.UTF8, CONTENTTYPE_APPLICATIONJSONPATCH);

            // Execute request
            if (ApiResponseFormat == ApiResponseFormat.modern)
                return await ExecuteAsync<Response>(request);
            else
                return await ExecuteAsync<bool>(request);
        }


        /// <summary>
        /// Validate a domain object.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public virtual IResponse Validate(TDto dto)
        {
            // Validation
            if (dto == null)
            {
                var errresp = new Response();
                errresp.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, nameof(dto)));
                return errresp;
            }

            // Create request
            HttpRequestMessage request = new HttpRequestMessage(
                HttpMethod.Post,
                $"{BaseUrl}/{ApiResource}/Validate?format={GetResponseFormat()}");
            string data = JsonSerializer.Instance.SerializeObject(dto);
            request.Content = new StringContent(data, Encoding.UTF8, CONTENTTYPE_APPLICATIONJSON);

            // Execute request
            if (ApiResponseFormat == ApiResponseFormat.modern)            
                return Execute<Response>(request);
            else
                return Execute<bool>(request);
        }


        /// <summary>
        /// Validate a domain object.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public virtual async Task<IResponse> ValidateAsync(TDto dto)
        {
            // Validation
            if (dto == null)
            {
                var errresp = new Response();
                errresp.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, nameof(dto)));
                return errresp;
            }

            // Create request
            HttpRequestMessage request = new HttpRequestMessage(
                HttpMethod.Post,
                $"{BaseUrl}/{ApiResource}/Validate?format={GetResponseFormat()}");
            string data = JsonSerializer.Instance.SerializeObject(dto);
            request.Content = new StringContent(data, Encoding.UTF8, CONTENTTYPE_APPLICATIONJSON);

            // Execute request
            if (ApiResponseFormat == ApiResponseFormat.modern)
                return await ExecuteAsync<Response>(request);
            else
                return await ExecuteAsync<bool>(request);
        }

    }
}