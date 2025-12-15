using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
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

        private static readonly ConcurrentDictionary<string, BearerTokenCredentials.CachedAccessToken> _tokenCache =
            new ConcurrentDictionary<string, BearerTokenCredentials.CachedAccessToken>();

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
                if (EnsureAccessToken() &&
                    _bearerTokenCredentials.AccessTokenResponse != null &&
                    !string.IsNullOrEmpty(_bearerTokenCredentials.AccessTokenResponse.access_token))
                {
                    if (!request.Headers.Contains(HEADER_AUTHORIZATION))
                    {
                        request.Headers.Add(
                            HEADER_AUTHORIZATION,
                            AUTHORIZATION_BEARER + " " + _bearerTokenCredentials.AccessTokenResponse.access_token);
                    }
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
                if (await EnsureAccessTokenAsync() &&
                    _bearerTokenCredentials.AccessTokenResponse != null &&
                    !string.IsNullOrEmpty(_bearerTokenCredentials.AccessTokenResponse.access_token))
                {
                    if (!request.Headers.Contains(HEADER_AUTHORIZATION))
                    {
                        request.Headers.Add(
                            HEADER_AUTHORIZATION,
                            AUTHORIZATION_BEARER + " " + _bearerTokenCredentials.AccessTokenResponse.access_token);
                    }
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

        /// <summary>
        /// Get the token cache key based on token config
        /// </summary>
        /// <returns></returns>
        private string GetTokenCacheKey()
        {
            if (_bearerTokenCredentials?.AccessTokenRequest == null)
                return null;

            var r = _bearerTokenCredentials.AccessTokenRequest;

            return string.Join("|",
                _bearerTokenCredentials.AuthorizationUrl ?? string.Empty,
                r.client_id ?? string.Empty,
                r.grant_type ?? string.Empty,
                r.response_type ?? string.Empty,
                r.scope ?? string.Empty);
        }

        /// <summary>
        /// Get from cache
        /// </summary>
        /// <returns></returns>
        private BearerTokenCredentials.CachedAccessToken TryGetCachedToken()
        {
            var key = GetTokenCacheKey();
            if (string.IsNullOrEmpty(key))
                return null;

            if (_tokenCache.TryGetValue(key, out var cached))
            {
                if (!cached.IsExpired)
                    return cached;

                // Remove expired token
                _tokenCache.TryRemove(key, out _);
            }

            return null;
        }

        /// <summary>
        /// Save the token
        /// </summary>
        /// <param name="tokenResponse"></param>
        private void StoreCacheToken(AccessTokenResponse tokenResponse)
        {
            if (tokenResponse == null)
                return;

            var key = GetTokenCacheKey();
            if (string.IsNullOrEmpty(key))
                return;

            var ttlSeconds = tokenResponse.expires_in > 0 ? tokenResponse.expires_in : 3600;
            // Subtract a small safety window
            var expiresAt = DateTimeOffset.UtcNow.AddSeconds(ttlSeconds - 60);

            var cached = new BearerTokenCredentials.CachedAccessToken
            {
                Response = tokenResponse,
                ExpiresAtUtc = expiresAt
            };

            _tokenCache[key] = cached;
        }

        /// <summary>
        /// Make sure we obtain token
        /// </summary>
        /// <returns></returns>
        private bool EnsureAccessToken()
        {
            if (_bearerTokenCredentials == null)
                return false;

            // Try static cache first
            var cached = TryGetCachedToken();
            if (cached != null)
            {
                _bearerTokenCredentials.AccessTokenResponse = cached.Response;
                return true;
            }

            // Fallback to existing token call (which hits the server)
            var respToken = GetAccessToken();
            if (respToken.Error || respToken.Item == null)
                return false;

            _bearerTokenCredentials.AccessTokenResponse = respToken.Item;
            StoreCacheToken(respToken.Item);
            return true;
        }

        /// <summary>
        /// Make sure we obtain token
        /// </summary>
        /// <returns></returns>
        private async Task<bool> EnsureAccessTokenAsync()
        {
            if (_bearerTokenCredentials == null)
                return false;

            // Try static cache first
            var cached = TryGetCachedToken();
            if (cached != null)
            {
                _bearerTokenCredentials.AccessTokenResponse = cached.Response;
                return true;
            }

            // Fallback to existing token call (which hits the server)
            var respToken = await GetAccessTokenAsync();
            if (respToken.Error || respToken.Item == null)
                return false;

            _bearerTokenCredentials.AccessTokenResponse = respToken.Item;
            StoreCacheToken(respToken.Item);
            return true;
        }
    }
}