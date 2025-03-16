using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServiceQuery;
using System.Text;

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
            HttpRequestMessage request = new HttpRequestMessage(
                HttpMethod.Get,
                $"{BaseUrl}/{ApiResource}/Get?storageKey={storageKey}");

            // Execute request
            if (_clientApiOptions.ReturnResponseObject)
            {
                var resp = Execute<ResponseItem<TDto>>(request);
                if (resp.Item != null)
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
            HttpRequestMessage request = new HttpRequestMessage(
                HttpMethod.Get,
                $"{BaseUrl}/{ApiResource}/GetAsync?storageKey={storageKey}");

            // Execute request
            if (_clientApiOptions.ReturnResponseObject)
            {
                var resp = await ExecuteAsync<ResponseItem<TDto>>(request);
                if (resp.Item != null)
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
                $"{BaseUrl}/{ApiResource}/Update");
            string data = JsonSerializer.Instance.SerializeObject(dto);
            request.Content = new StringContent(data, Encoding.UTF8, CONTENTTYPE_APPLICATIONJSON);

            // Execute request
            if (_clientApiOptions.ReturnResponseObject)
            {
                var resp = Execute<ResponseItem<TDto>>(request);
                if (resp.Item != null)
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
                $"{BaseUrl}/{ApiResource}/UpdateAsync");
            string data = JsonSerializer.Instance.SerializeObject(dto);
            request.Content = new StringContent(data, Encoding.UTF8, CONTENTTYPE_APPLICATIONJSON);

            // Execute request
            if (_clientApiOptions.ReturnResponseObject)
            {
                var resp = await ExecuteAsync<ResponseItem<TDto>>(request);
                if (resp.Item != null)
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
                $"{BaseUrl}/{ApiResource}/Create");
            string data = JsonSerializer.Instance.SerializeObject(dto);
            request.Content = new StringContent(data, Encoding.UTF8, CONTENTTYPE_APPLICATIONJSON);

            // Execute request
            if (_clientApiOptions.ReturnResponseObject)
            {
                var resp = Execute<ResponseItem<TDto>>(request);
                if (resp.Item != null)
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
                $"{BaseUrl}/{ApiResource}/CreateAsync");
            string data = JsonSerializer.Instance.SerializeObject(dto);
            request.Content = new StringContent(data, Encoding.UTF8, CONTENTTYPE_APPLICATIONJSON);

            // Execute request
            if (_clientApiOptions.ReturnResponseObject)
            {
                var resp = await ExecuteAsync<ResponseItem<TDto>>(request);
                if (resp.Item != null)
                    return resp.Item;

                ResponseItem<TDto> respEr = new ResponseItem<TDto>();
                respEr.CopyFrom(resp);
                return respEr;
            }
            else
                return await ExecuteAsync<TDto>(request);
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
                $"{BaseUrl}/{ApiResource}/Delete?storageKey={storageKey}");

            // Execute request
            if (_clientApiOptions.ReturnResponseObject)
            {
                return Execute<Response>(request);
            }
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
            HttpRequestMessage request = new HttpRequestMessage(
                HttpMethod.Delete,
                $"{BaseUrl}/{ApiResource}/DeleteAsync?storageKey={storageKey}");

            // Execute request
            if (_clientApiOptions.ReturnResponseObject)
            {
                return await ExecuteAsync<Response>(request);
            }
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
                $"{BaseUrl}/{ApiResource}/Query");
            string data = JsonSerializer.Instance.SerializeObject(serviceQueryRequest);
            request.Content = new StringContent(data, Encoding.UTF8, CONTENTTYPE_APPLICATIONJSON);

            // Execute request
            if (_clientApiOptions.ReturnResponseObject)
            {
                var resp = Execute<ResponseItem<ServiceQueryResponse<TDto>>>(request);
                if (resp.Item != null)
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
                $"{BaseUrl}/{ApiResource}/QueryAsync");
            string data = JsonSerializer.Instance.SerializeObject(serviceQueryRequest);
            request.Content = new StringContent(data, Encoding.UTF8, CONTENTTYPE_APPLICATIONJSON);

            // Execute request
            if (_clientApiOptions.ReturnResponseObject)
            {
                var resp = await ExecuteAsync<ResponseItem<ServiceQueryResponse<TDto>>>(request);
                if (resp.Item != null)
                    return resp.Item;

                ResponseItem<ServiceQueryResponse<TDto>> respEr = new ResponseItem<ServiceQueryResponse<TDto>>();
                respEr.CopyFrom(resp);
                return respEr;
            }
            else
                return await ExecuteAsync<ServiceQueryResponse<TDto>>(request);
        }
    }
}