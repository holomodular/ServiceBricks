using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ServiceQuery;
using System.Text;

namespace ServiceBricks
{
    /// <summary>
    /// This is a REST-based service client for a domain object based API service.
    /// </summary>
    /// <typeparam name="TDto"></typeparam>
    public class ApiClient<TDto> : BearerTokenClient, IApiClient<TDto>
        where TDto : class, IDataTransferObject
    {
        protected readonly ApiConfig _apiConfig;
        protected readonly ILogger<ApiClient<TDto>> _logger;
        protected Type _type = null;

        public ApiClient(
            ILoggerFactory loggerFactory,
            IHttpClientFactory httpClientFactory,
            ApiConfig apiConfig)
            : base(loggerFactory, httpClientFactory, apiConfig.DisableAuthentication ? null : new BearerTokenCredentials()
            {
                AccessTokenRequest = new AccessTokenRequest()
                {
                    client_id = apiConfig.TokenClient,
                    client_secret = apiConfig.TokenSecret,
                    grant_type = apiConfig.TokenType,
                    response_type = apiConfig.TokenResponseType,
                    scope = apiConfig.TokenScope,
                },
                AuthorizationUrl = apiConfig.TokenUrl,
            })
        {
            _logger = loggerFactory.CreateLogger<ApiClient<TDto>>();
            _apiConfig = apiConfig ?? new ApiConfig();
            _type = typeof(TDto);
            BaseUrl = _apiConfig.BaseServiceUrl;
        }

        /// <summary>
        /// This the base url of the API request.
        /// </summary>
        public string BaseUrl { get; set; }

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
            return ExecuteAsync<TModel>(request).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Execute the request async.
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        public virtual async Task<IResponseItem<TModel>> ExecuteAsync<TModel>(HttpRequestMessage request)
        {
            var resp = new ResponseItem<TModel>();
            if (request == null)
            {
                resp.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, nameof(request)));
                return resp;
            }
            try
            {
                var result = await SendAsync(request);
                if (result != null)
                {
                    if (result.IsSuccessStatusCode)
                    {
                        var content = await result.Content.ReadAsStringAsync();
                        if (string.IsNullOrEmpty(content))
                            resp.Item = default(TModel);
                        else
                            resp.Item = JsonConvert.DeserializeObject<TModel>(content);
                        return resp;
                    }
                    else
                    {
                        if (_apiConfig.ReturnResponseObject)
                        {
                            var content = await result.Content.ReadAsStringAsync();
                            if (string.IsNullOrEmpty(content))
                                resp.Item = default(TModel);
                            else
                            {
                                resp.Item = JsonConvert.DeserializeObject<TModel>(content);
                            }
                            return resp;
                        }
                        else
                        {
                            resp.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_REST_CLIENT));
                            return resp;
                        }
                    }
                }
                else
                {
                    resp.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_REST_CLIENT));
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
            HttpRequestMessage request = new HttpRequestMessage(
                HttpMethod.Get,
                $"{BaseUrl}/{ApiResource}/Get?storageKey={storageKey}");
            if (_apiConfig.ReturnResponseObject)
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
            HttpRequestMessage request = new HttpRequestMessage(
                HttpMethod.Get,
                $"{BaseUrl}/{ApiResource}/GetAsync?storageKey={storageKey}");
            if (_apiConfig.ReturnResponseObject)
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
            HttpRequestMessage request = new HttpRequestMessage(
                HttpMethod.Put,
                $"{BaseUrl}/{ApiResource}/Update");

            string data = JsonConvert.SerializeObject(dto);
            request.Content = new StringContent(data, Encoding.UTF8, CONTENTTYPE_APPLICATIONJSON);
            if (_apiConfig.ReturnResponseObject)
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
            HttpRequestMessage request = new HttpRequestMessage(
                HttpMethod.Put,
                $"{BaseUrl}/{ApiResource}/UpdateAsync");

            string data = JsonConvert.SerializeObject(dto);
            request.Content = new StringContent(data, Encoding.UTF8, CONTENTTYPE_APPLICATIONJSON);
            if (_apiConfig.ReturnResponseObject)
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
            HttpRequestMessage request = new HttpRequestMessage(
                HttpMethod.Post,
                $"{BaseUrl}/{ApiResource}/Create");

            string data = JsonConvert.SerializeObject(dto);
            request.Content = new StringContent(data, Encoding.UTF8, CONTENTTYPE_APPLICATIONJSON);
            if (_apiConfig.ReturnResponseObject)
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
            HttpRequestMessage request = new HttpRequestMessage(
                HttpMethod.Post,
                $"{BaseUrl}/{ApiResource}/CreateAsync");

            string data = JsonConvert.SerializeObject(dto);
            request.Content = new StringContent(data, Encoding.UTF8, CONTENTTYPE_APPLICATIONJSON);
            if (_apiConfig.ReturnResponseObject)
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
            HttpRequestMessage request = new HttpRequestMessage(
                HttpMethod.Delete,
                $"{BaseUrl}/{ApiResource}/Delete?storageKey={storageKey}");
            if (_apiConfig.ReturnResponseObject)
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
            HttpRequestMessage request = new HttpRequestMessage(
                HttpMethod.Delete,
                $"{BaseUrl}/{ApiResource}/DeleteAsync?storageKey={storageKey}");
            if (_apiConfig.ReturnResponseObject)
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
        public virtual IResponseItem<ServiceQueryResponse<TDto>> Query(ServiceQueryRequest req)
        {
            HttpRequestMessage request = new HttpRequestMessage(
                HttpMethod.Post,
                $"{BaseUrl}/{ApiResource}/Query");

            string data = JsonConvert.SerializeObject(req);
            request.Content = new StringContent(data, Encoding.UTF8, CONTENTTYPE_APPLICATIONJSON);
            if (_apiConfig.ReturnResponseObject)
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
        public virtual async Task<IResponseItem<ServiceQueryResponse<TDto>>> QueryAsync(ServiceQueryRequest req)
        {
            HttpRequestMessage request = new HttpRequestMessage(
                HttpMethod.Post,
                $"{BaseUrl}/{ApiResource}/QueryAsync");

            string data = JsonConvert.SerializeObject(req);
            request.Content = new StringContent(data, Encoding.UTF8, CONTENTTYPE_APPLICATIONJSON);
            if (_apiConfig.ReturnResponseObject)
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