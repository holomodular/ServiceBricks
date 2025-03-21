﻿using Microsoft.Extensions.Logging;
using System.Text;

namespace ServiceBricks
{
    /// <summary>
    /// This is a service client that obtains a bearer token.
    /// </summary>
    public partial class BearerTokenClient : ServiceClient
    {
        private readonly ILogger<BearerTokenClient> _logger;

        protected BearerTokenCredentials _bearerTokenCredentials;

        /// <summary>
        /// Constants for authorization.
        /// </summary>
        public const string HEADER_AUTHORIZATION = "Authorization";

        /// <summary>
        /// Constants for bearer.
        /// </summary>
        public const string AUTHORIZATION_BEARER = "Bearer";

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="loggerFactory"></param>
        /// <param name="httpClientFactory"></param>
        /// <param name="bearerTokenCredentials"></param>
        public BearerTokenClient(
            ILoggerFactory loggerFactory,
            IHttpClientFactory httpClientFactory,
            BearerTokenCredentials bearerTokenCredentials)
            : base(httpClientFactory)
        {
            _logger = loggerFactory.CreateLogger<BearerTokenClient>();
            _bearerTokenCredentials = bearerTokenCredentials;
        }

        /// <summary>
        /// Send a request with a bearer token.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public override HttpResponseMessage Send(HttpRequestMessage request)
        {
            if (request == null)
                return null;

            if (_bearerTokenCredentials != null)
            {
                if (_bearerTokenCredentials.AccessTokenResponse == null || string.IsNullOrEmpty(_bearerTokenCredentials.AccessTokenResponse.access_token))
                {
                    var respToken = GetAccessToken();
                    if (respToken.Success && respToken.Item != null)
                        _bearerTokenCredentials.AccessTokenResponse = respToken.Item;
                }

                if (_bearerTokenCredentials.AccessTokenResponse != null && !string.IsNullOrEmpty(_bearerTokenCredentials.AccessTokenResponse.access_token))
                {
                    if (!request.Headers.Contains(HEADER_AUTHORIZATION))
                        request.Headers.Add(HEADER_AUTHORIZATION, AUTHORIZATION_BEARER + " " + _bearerTokenCredentials.AccessTokenResponse.access_token);
                }
            }

            return base.Send(request);
        }

        /// <summary>
        /// Send a request with a bearer token.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            if (request == null)
                return null;

            if (_bearerTokenCredentials != null)
            {
                if (_bearerTokenCredentials.AccessTokenResponse == null || string.IsNullOrEmpty(_bearerTokenCredentials.AccessTokenResponse.access_token))
                {
                    var respToken = await GetAccessTokenAsync();
                    if (respToken.Success && respToken.Item != null)
                        _bearerTokenCredentials.AccessTokenResponse = respToken.Item;
                }

                if (_bearerTokenCredentials.AccessTokenResponse != null && !string.IsNullOrEmpty(_bearerTokenCredentials.AccessTokenResponse.access_token))
                {
                    if (!request.Headers.Contains(HEADER_AUTHORIZATION))
                        request.Headers.Add(HEADER_AUTHORIZATION, AUTHORIZATION_BEARER + " " + _bearerTokenCredentials.AccessTokenResponse.access_token);
                }
            }

            return await base.SendAsync(request);
        }

        /// <summary>
        /// Get the access token.
        /// </summary>
        /// <returns></returns>
        public virtual IResponseItem<AccessTokenResponse> GetAccessToken()
        {
            var response = new ResponseItem<AccessTokenResponse>();
            if (_bearerTokenCredentials == null || _bearerTokenCredentials.AccessTokenRequest == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING));
                return response;
            }

            try
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, _bearerTokenCredentials.AuthorizationUrl);
                string data = JsonSerializer.Instance.SerializeObject(_bearerTokenCredentials.AccessTokenRequest);
                request.Content = new StringContent(data, Encoding.UTF8, CONTENTTYPE_APPLICATIONJSON);
                var result = base.Send(request);
                if (result != null && result.IsSuccessStatusCode)
                {
                    using (var stream = result.Content.ReadAsStream())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            string content = reader.ReadToEnd();
                            response.Item = JsonSerializer.Instance.DeserializeObject<AccessTokenResponse>(content);
                        }
                    }
                }
                else
                    response.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_REST_CLIENT));
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                response.AddMessage(ResponseMessage.CreateError(ex, LocalizationResource.ERROR_REST_CLIENT));
                return response;
            }
        }

        /// <summary>
        /// Get the access token asynchronously.
        /// </summary>
        /// <returns></returns>
        public virtual async Task<IResponseItem<AccessTokenResponse>> GetAccessTokenAsync()
        {
            var response = new ResponseItem<AccessTokenResponse>();
            if (_bearerTokenCredentials == null || _bearerTokenCredentials.AccessTokenRequest == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING));
                return response;
            }

            try
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, _bearerTokenCredentials.AuthorizationUrl);
                string data = JsonSerializer.Instance.SerializeObject(_bearerTokenCredentials.AccessTokenRequest);
                request.Content = new StringContent(data, Encoding.UTF8, CONTENTTYPE_APPLICATIONJSON);
                var result = await base.SendAsync(request);
                if (result != null && result.IsSuccessStatusCode)
                {
                    var content = await result.Content.ReadAsStringAsync();
                    response.Item = JsonSerializer.Instance.DeserializeObject<AccessTokenResponse>(content);
                }
                else
                    response.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_REST_CLIENT));
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                response.AddMessage(ResponseMessage.CreateError(ex, LocalizationResource.ERROR_REST_CLIENT));
                return response;
            }
        }
    }
}