using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ServiceQuery;
using System.Net;

namespace ServiceBricks.Xunit
{
    [Collection(Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public partial class ApiClientTests
    {
        public virtual ISystemManager SystemManager { get; set; }

        public ApiClientTests()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(ServiceBricksStartup));
        }

        public class ExampleApiClient : ApiClient<ExampleDto>
        {
            public ExampleApiClient(
                ILoggerFactory loggerFactory,
                IHttpClientFactory httpClientFactory,
                ClientApiOptions clientApiOptions)
                : base(loggerFactory, httpClientFactory, clientApiOptions)
            {
                ApiResource = "Example/Example";
            }
        }

        public class ReturnNullHttpClientHandler : HttpClientHandler
        {
            protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                return null;
            }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                return null;
            }
        }

        public class CustomGenericHttpClientHandler<TDto> : HttpClientHandler where TDto : class, IDataTransferObject

        {
            private IApiController<TDto> _service;

            public CustomGenericHttpClientHandler(
                IApiController<TDto> service)
            {
                _service = service;
            }

            public bool GetAccessTokenCalled { get; set; }
            public bool SendCalled { get; set; }
            public bool CreateCalled { get; set; }
            public bool GetCalled { get; set; }
            public bool UpdateCalled { get; set; }
            public bool DeleteCalled { get; set; }
            public bool QueryCalled { get; set; }
            public bool PatchCalled { get; set; }
            public bool CreateAckCalled { get; set; }            
            public bool UpdateAckCalled { get; set; }                        
            public bool PatchAckCalled { get; set; }
            public bool ValidateCalled { get; set; }

            protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                SendCalled = true;

                string url = request.RequestUri.ToString();
                string content = string.Empty;
                if (request.Content != null)
                    content = request.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                string format = "modern";
                if(url.Contains("format=", StringComparison.InvariantCultureIgnoreCase))
                    format = url.Substring(url.IndexOf("format=", StringComparison.InvariantCultureIgnoreCase) + 7);
                if (request.Method == HttpMethod.Get)
                {                    
                    GetCalled = true;
                    if (_service != null && url.Contains("storageKey=", StringComparison.InvariantCultureIgnoreCase))
                    {
                        var storageKey = url.Substring(url.IndexOf("storageKey=", StringComparison.InvariantCultureIgnoreCase) + 11);
                        var resp = _service.Get(storageKey);
                        return GetClientResponse(resp);
                    }
                }
                else if (request.Method == HttpMethod.Put)
                {
                    if (url.Contains("updateack", StringComparison.InvariantCultureIgnoreCase))
                    {
                        UpdateAckCalled = true;
                        if (_service != null)
                        {
                            var dto = JsonSerializer.Instance.DeserializeObject<TDto>(content);
                            var resp = _service.UpdateAck(dto);
                            return GetClientResponse(resp);
                        }
                    }
                    else
                    {
                        UpdateCalled = true;
                        if (_service != null)
                        {
                            var dto = JsonSerializer.Instance.DeserializeObject<TDto>(content);
                            var resp = _service.Update(dto);
                            return GetClientResponse(resp);
                        }
                    }
                }
                else if (request.Method == HttpMethod.Delete)
                {
                    DeleteCalled = true;
                    if (_service != null && url.Contains("storageKey=", StringComparison.InvariantCultureIgnoreCase))
                    {
                        var storageKey = url.Substring(url.IndexOf("storageKey=", StringComparison.InvariantCultureIgnoreCase) + 11);
                        var resp = _service.Delete(storageKey);
                        return GetClientResponse(resp);
                    }
                }
                else if (request.Method == HttpMethod.Patch)
                {
                    
                    if (url.Contains("patchack", StringComparison.InvariantCultureIgnoreCase))
                    {
                        PatchAckCalled = true;
                        if (_service != null && url.Contains("storageKey=", StringComparison.InvariantCultureIgnoreCase))
                        {
                            var storageKey = url.Substring(url.IndexOf("storageKey=", StringComparison.InvariantCultureIgnoreCase) + 11);
                            var obj = JsonSerializer.Instance.DeserializeObject<Microsoft.AspNetCore.JsonPatch.JsonPatchDocument<TDto>>(content);
                            var resp = _service.PatchAck(storageKey, obj);
                            return GetClientResponse(resp);
                        }
                    }
                    else
                    {
                        PatchCalled = true;
                        if (_service != null && url.Contains("storageKey=", StringComparison.InvariantCultureIgnoreCase))
                        {
                            var storageKey = url.Substring(url.IndexOf("storageKey=", StringComparison.InvariantCultureIgnoreCase) + 11);
                            var obj = JsonSerializer.Instance.DeserializeObject<Microsoft.AspNetCore.JsonPatch.JsonPatchDocument<TDto>>(content);
                            var resp = _service.Patch(storageKey, obj);
                            return GetClientResponse(resp);
                        }
                    }
                    
                }
                else if (request.Method == HttpMethod.Post)
                {
                    if (url.Contains("validate", StringComparison.InvariantCultureIgnoreCase))
                    {
                        ValidateCalled = true;
                        if (_service != null)
                        {
                            var dto = JsonSerializer.Instance.DeserializeObject<TDto>(content);
                            var resp = _service.Validate(dto);
                            return GetClientResponse(resp);
                        }
                    }
                    if (url.Contains("query", StringComparison.InvariantCultureIgnoreCase))
                    {
                        QueryCalled = true;
                        if (_service != null)
                        {
                            var dto = JsonSerializer.Instance.DeserializeObject<ServiceQueryRequest>(content);
                            var resp = _service.Query(dto);
                            return GetClientResponse(resp);
                        }
                    }
                    if (url.Contains("createack", StringComparison.InvariantCultureIgnoreCase))
                    {
                        CreateAckCalled = true;
                        if (_service != null)
                        {
                            var dto = JsonSerializer.Instance.DeserializeObject<TDto>(content);
                            var resp = _service.CreateAck(dto);
                            return GetClientResponse(resp);
                        }
                    }
                    if (url.Contains("create", StringComparison.InvariantCultureIgnoreCase))
                    {
                        CreateCalled = true;
                        if (_service != null)
                        {
                            var dto = JsonSerializer.Instance.DeserializeObject<TDto>(content);
                            var resp = _service.Create(dto);
                            return GetClientResponse(resp);
                        }
                    }
                    else if (url.Contains("token", StringComparison.InvariantCultureIgnoreCase))
                    {
                        GetAccessTokenCalled = true;

                        AccessTokenResponse resp = new AccessTokenResponse();
                        resp.access_token = Guid.NewGuid().ToString();
                        resp.expires_in = 3600;
                        return GetClientResponse(resp);
                    }
                }
                return new HttpResponseMessage();
            }

            public HttpResponseMessage GetClientResponse(object response)
            {
                Microsoft.AspNetCore.Mvc.ObjectResult or = response as Microsoft.AspNetCore.Mvc.ObjectResult;
                if (or != null)
                {

                    HttpStatusCode code = HttpStatusCode.OK;
                    if (or.StatusCode.HasValue && or.StatusCode.Value == 201)
                        code = HttpStatusCode.Created;
                    if (or.StatusCode.HasValue && or.StatusCode.Value == 400)
                        code = HttpStatusCode.BadRequest;
                    return new HttpResponseMessage()
                    {
                        Content = new StringContent(JsonSerializer.Instance.SerializeObject(or.Value)),
                        StatusCode = code
                    };
                }
                return new HttpResponseMessage()
                {
                    Content = new StringContent(JsonSerializer.Instance.SerializeObject(response))
                };
            }

            public bool GetAccessTokenAsyncCalled { get; set; }
            public bool SendAsyncCalled { get; set; }
            public bool CreateAsyncCalled { get; set; }
            public bool GetAsyncCalled { get; set; }
            public bool UpdateAsyncCalled { get; set; }
            public bool DeleteAsyncCalled { get; set; }
            public bool QueryAsyncCalled { get; set; }
            public bool PatchAsyncCalled { get; set; }
            public bool CreateAckAsyncCalled { get; set; }            
            public bool UpdateAckAsyncCalled { get; set; }                        
            public bool PatchAckAsyncCalled { get; set; }
            public bool ValidateAsyncCalled { get; set; }

            protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                await Task.CompletedTask;

                SendAsyncCalled = true;

                string url = request.RequestUri.ToString();
                string content = string.Empty;
                if (request.Content != null)
                    content = request.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                if (request.Method == HttpMethod.Get)
                {
                    GetAsyncCalled = true;
                    if (_service != null && url.Contains("storageKey=", StringComparison.InvariantCultureIgnoreCase))
                    {
                        var storageKey = url.Substring(url.IndexOf("storageKey=", StringComparison.InvariantCultureIgnoreCase) + 11);
                        var resp = await _service.GetAsync(storageKey);
                        return GetClientResponse(resp);
                    }
                }
                else if (request.Method == HttpMethod.Put)
                {
                    if (url.Contains("updateack", StringComparison.InvariantCultureIgnoreCase))
                    {
                        UpdateAckAsyncCalled = true;

                        if (_service != null)
                        {
                            var dto = JsonSerializer.Instance.DeserializeObject<TDto>(content);
                            var resp = await _service.UpdateAckAsync(dto);
                            return GetClientResponse(resp);
                        }
                    }
                    else
                    {
                        UpdateAsyncCalled = true;

                        if (_service != null)
                        {
                            var dto = JsonSerializer.Instance.DeserializeObject<TDto>(content);
                            var resp = await _service.UpdateAsync(dto);
                            return GetClientResponse(resp);
                        }
                    }
                }
                else if (request.Method == HttpMethod.Delete)
                {
                    DeleteAsyncCalled = true;
                    if (_service != null && url.Contains("storageKey=", StringComparison.InvariantCultureIgnoreCase))
                    {
                        var storageKey = url.Substring(url.IndexOf("storageKey=", StringComparison.InvariantCultureIgnoreCase) + 11);
                        var resp = _service.Delete(storageKey);
                        return GetClientResponse(resp);
                    }
                }
                else if (request.Method == HttpMethod.Patch)
                {
                    if(url.Contains("patchack",StringComparison.InvariantCultureIgnoreCase))
                    {
                        PatchAckAsyncCalled = true;
                        if (_service != null && url.Contains("storageKey=", StringComparison.InvariantCultureIgnoreCase))
                        {
                            var storageKey = url.Substring(url.IndexOf("storageKey=", StringComparison.InvariantCultureIgnoreCase) + 11);
                            var obj = JsonSerializer.Instance.DeserializeObject<Microsoft.AspNetCore.JsonPatch.JsonPatchDocument<TDto>>(content);
                            var resp = await _service.PatchAckAsync(storageKey, obj);
                            return GetClientResponse(resp);
                        }
                    }
                    else
                    {
                        PatchAsyncCalled = true;
                        if (_service != null && url.Contains("storageKey=", StringComparison.InvariantCultureIgnoreCase))
                        {
                            var storageKey = url.Substring(url.IndexOf("storageKey=", StringComparison.InvariantCultureIgnoreCase) + 11);
                            var obj = JsonSerializer.Instance.DeserializeObject<Microsoft.AspNetCore.JsonPatch.JsonPatchDocument<TDto>>(content);
                            var resp = await _service.PatchAsync(storageKey, obj);
                            return GetClientResponse(resp);
                        }
                    }
                        
                }
                else if (request.Method == HttpMethod.Post)
                {
                    if (url.Contains("validate", StringComparison.InvariantCultureIgnoreCase))
                    {
                        ValidateAsyncCalled = true;
                        if (_service != null)
                        {
                            var dto = JsonSerializer.Instance.DeserializeObject<TDto>(content);
                            var resp = await _service.ValidateAsync(dto);
                            return GetClientResponse(resp);
                        }
                    }
                    if (url.Contains("query", StringComparison.InvariantCultureIgnoreCase))
                    {
                        QueryAsyncCalled = true;
                        if (_service != null)
                        {
                            var dto = JsonSerializer.Instance.DeserializeObject<ServiceQueryRequest>(content);
                            var resp = await _service.QueryAsync(dto);
                            return GetClientResponse(resp);
                        }
                    }
                    if (url.Contains("createack", StringComparison.InvariantCultureIgnoreCase))
                    {
                        CreateAckAsyncCalled = true;
                        if (_service != null)
                        {
                            var dto = JsonSerializer.Instance.DeserializeObject<TDto>(content);
                            var resp = await _service.CreateAckAsync(dto);
                            return GetClientResponse(resp);
                        }
                    }
                    if (url.Contains("create", StringComparison.InvariantCultureIgnoreCase))
                    {
                        CreateAsyncCalled = true;
                        if (_service != null)
                        {
                            var dto = JsonSerializer.Instance.DeserializeObject<TDto>(content);
                            var resp = await _service.CreateAsync(dto);
                            return GetClientResponse(resp);
                        }
                    }
                    else if (url.Contains("token", StringComparison.InvariantCultureIgnoreCase))
                    {
                        GetAccessTokenAsyncCalled = true;

                        AccessTokenResponse resp = new AccessTokenResponse();
                        resp.access_token = Guid.NewGuid().ToString();
                        resp.expires_in = 3600;
                        return GetClientResponse(resp);
                    }
                }
                return new HttpResponseMessage();
            }
        }

        public class ExampleHttpClientFactory : IHttpClientFactory
        {
            private HttpClientHandler _handler;

            public ExampleHttpClientFactory(HttpClientHandler handler)
            {
                _handler = handler;
            }

            public HttpClient CreateClient(string name)
            {
                return new HttpClient(_handler);
            }
        }

        [Fact]
        public virtual Task ApiClientCallNoAuthSuccess()
        {
            var loggerFactory = SystemManager.ServiceProvider.GetRequiredService<ILoggerFactory>();
            var options = new ClientApiOptions()
            {
                BaseServiceUrl = "https://localhost:7000/api/v1.0",
                DisableAuthentication = true,
                ReturnResponseObject = false,
                TokenUrl = "https://localhost:7000/api/v1.0/token",
                TokenClient = "ApiClientCallNoAuthSuccess"
            };
            var handler = new CustomGenericHttpClientHandler<ExampleDto>(null);
            var handlerFactory = new ExampleHttpClientFactory(handler);
            var client = new ExampleApiClient(
                loggerFactory,
                handlerFactory,
                options);

            var dto = new ExampleDto() { StorageKey = "1" };

            // Get
            Assert.True(!handler.GetCalled);
            client.Get(dto.StorageKey);
            Assert.True(handler.SendCalled);
            Assert.True(handler.GetCalled);
            Assert.True(!handler.GetAccessTokenCalled);
            handler.SendCalled = false;
            handler.GetCalled = false;

            // Create
            Assert.True(!handler.CreateCalled);
            client.Create(dto);
            Assert.True(handler.SendCalled);
            Assert.True(handler.CreateCalled);
            Assert.True(!handler.GetAccessTokenCalled);
            handler.CreateCalled = false;

            // CreateAck
            Assert.True(!handler.CreateAckCalled);
            client.CreateAck(dto);
            Assert.True(handler.SendCalled);
            Assert.True(handler.CreateAckCalled);
            Assert.True(!handler.GetAccessTokenCalled);
            handler.CreateAckCalled = false;

            // Update
            Assert.True(!handler.UpdateCalled);
            client.Update(dto);
            Assert.True(handler.SendCalled);
            Assert.True(handler.UpdateCalled);
            Assert.True(!handler.GetAccessTokenCalled);
            handler.UpdateCalled = false;

            // UpdateAck
            Assert.True(!handler.UpdateAckCalled);
            client.UpdateAck(dto);
            Assert.True(handler.SendCalled);
            Assert.True(handler.UpdateAckCalled);
            Assert.True(!handler.GetAccessTokenCalled);
            handler.UpdateAckCalled = false;

            // Delete
            Assert.True(!handler.DeleteCalled);
            client.Delete(dto.StorageKey);
            Assert.True(handler.SendCalled);
            Assert.True(handler.DeleteCalled);
            Assert.True(!handler.GetAccessTokenCalled);
            handler.DeleteCalled = false;

            // Query
            Assert.True(!handler.QueryCalled);
            client.Query(new ServiceQueryRequest());
            Assert.True(handler.SendCalled);
            Assert.True(handler.QueryCalled);
            Assert.True(!handler.GetAccessTokenCalled);
            handler.QueryCalled = false;

            // Patch
            Assert.True(!handler.PatchCalled);
            client.Patch(dto.StorageKey, new Microsoft.AspNetCore.JsonPatch.JsonPatchDocument<ExampleDto>());
            Assert.True(handler.SendCalled);
            Assert.True(handler.PatchCalled);
            Assert.True(!handler.GetAccessTokenCalled);
            handler.PatchCalled = false;

            // PatchAck
            Assert.True(!handler.PatchAckCalled);
            client.PatchAck(dto.StorageKey, new Microsoft.AspNetCore.JsonPatch.JsonPatchDocument<ExampleDto>());
            Assert.True(handler.SendCalled);
            Assert.True(handler.PatchAckCalled);
            Assert.True(!handler.GetAccessTokenCalled);
            handler.PatchAckCalled = false;

            // Validate
            Assert.True(!handler.ValidateCalled);
            client.Validate(dto);
            Assert.True(handler.SendCalled);
            Assert.True(handler.ValidateCalled);
            Assert.True(!handler.GetAccessTokenCalled);
            handler.ValidateCalled = false;


            return Task.CompletedTask;
        }

        [Fact]
        public virtual async Task ApiClientCallNoAuthSuccessAsync()
        {
            var loggerFactory = SystemManager.ServiceProvider.GetRequiredService<ILoggerFactory>();
            var options = new ClientApiOptions()
            {
                BaseServiceUrl = "https://localhost:7000/api/v1.0",
                DisableAuthentication = true,
                ReturnResponseObject = false,
                TokenUrl = "https://localhost:7000/api/v1.0/token",
                TokenClient = "ApiClientCallNoAuthSuccessAsync"
            };
            var handler = new CustomGenericHttpClientHandler<ExampleDto>(null);
            var handlerFactory = new ExampleHttpClientFactory(handler);
            var client = new ExampleApiClient(
                loggerFactory,
                handlerFactory,
                options);

            var dto = new ExampleDto() { StorageKey = "1" };

            // Get
            Assert.True(!handler.GetAsyncCalled);
            await client.GetAsync(dto.StorageKey);
            Assert.True(handler.SendAsyncCalled);
            Assert.True(handler.GetAsyncCalled);
            Assert.True(!handler.GetAccessTokenAsyncCalled);
            handler.SendAsyncCalled = false;
            handler.GetAsyncCalled = false;

            // Create
            Assert.True(!handler.CreateAsyncCalled);
            await client.CreateAsync(dto);
            Assert.True(handler.SendAsyncCalled);
            Assert.True(handler.CreateAsyncCalled);
            Assert.True(!handler.GetAccessTokenAsyncCalled);
            handler.CreateAsyncCalled = false;

            // CreateAck
            Assert.True(!handler.CreateAckAsyncCalled);
            await client.CreateAckAsync(dto);
            Assert.True(handler.SendAsyncCalled);
            Assert.True(handler.CreateAckAsyncCalled);
            Assert.True(!handler.GetAccessTokenAsyncCalled);
            handler.CreateAckAsyncCalled = false;

            // Update
            Assert.True(!handler.UpdateAsyncCalled);
            await client.UpdateAsync(dto);
            Assert.True(handler.SendAsyncCalled);
            Assert.True(handler.UpdateAsyncCalled);
            Assert.True(!handler.GetAccessTokenAsyncCalled);
            handler.UpdateAsyncCalled = false;

            // UpdateAck
            Assert.True(!handler.UpdateAckAsyncCalled);
            await client.UpdateAckAsync(dto);
            Assert.True(handler.SendAsyncCalled);
            Assert.True(handler.UpdateAckAsyncCalled);
            Assert.True(!handler.GetAccessTokenAsyncCalled);
            handler.UpdateAckAsyncCalled = false;

            // Delete
            Assert.True(!handler.DeleteAsyncCalled);
            await client.DeleteAsync(dto.StorageKey);
            Assert.True(handler.SendAsyncCalled);
            Assert.True(handler.DeleteAsyncCalled);
            Assert.True(!handler.GetAccessTokenAsyncCalled);
            handler.DeleteAsyncCalled = false;

            // Query
            Assert.True(!handler.QueryAsyncCalled);
            await client.QueryAsync(new ServiceQueryRequest());
            Assert.True(handler.SendAsyncCalled);
            Assert.True(handler.QueryAsyncCalled);
            Assert.True(!handler.GetAccessTokenAsyncCalled);
            handler.QueryAsyncCalled = false;

            // Patch
            Assert.True(!handler.PatchAsyncCalled);
            await client.PatchAsync(dto.StorageKey, new Microsoft.AspNetCore.JsonPatch.JsonPatchDocument<ExampleDto>());
            Assert.True(handler.SendAsyncCalled);
            Assert.True(handler.PatchAsyncCalled);
            Assert.True(!handler.GetAccessTokenAsyncCalled);
            handler.PatchAsyncCalled = false;

            // PatchAck
            Assert.True(!handler.PatchAckAsyncCalled);
            await client.PatchAckAsync(dto.StorageKey, new Microsoft.AspNetCore.JsonPatch.JsonPatchDocument<ExampleDto>());
            Assert.True(handler.SendAsyncCalled);
            Assert.True(handler.PatchAckAsyncCalled);
            Assert.True(!handler.GetAccessTokenAsyncCalled);
            handler.PatchAckAsyncCalled = false;

            // Validate
            Assert.True(!handler.ValidateAsyncCalled);
            await client.ValidateAsync(dto);
            Assert.True(handler.SendAsyncCalled);
            Assert.True(handler.ValidateAsyncCalled);
            Assert.True(!handler.GetAccessTokenAsyncCalled);
            handler.ValidateAsyncCalled = false;
        }

        [Fact]
        public virtual Task ApiClientCallWithAuthSuccess()
        {
            var loggerFactory = SystemManager.ServiceProvider.GetRequiredService<ILoggerFactory>();
            var options = new ClientApiOptions()
            {
                BaseServiceUrl = "https://localhost:7000/api/v1.0",
                DisableAuthentication = false,
                ReturnResponseObject = false,
                TokenUrl = "https://localhost:7000/api/v1.0/token",
                TokenClient = "ApiClientCallWithAuthSuccess"
            };
            var handler = new CustomGenericHttpClientHandler<ExampleDto>(null);
            var handlerFactory = new ExampleHttpClientFactory(handler);
            var client = new ExampleApiClient(
                loggerFactory,
                handlerFactory,
                options);

            var dto = new ExampleDto() { StorageKey = "1" };

            // Make sure reset
            handler.GetAccessTokenCalled = false;
            handler.SendCalled = false;

            // Get
            Assert.True(!handler.GetAsyncCalled);
            client.Get(dto.StorageKey);
            Assert.True(handler.SendCalled);
            Assert.True(handler.GetAccessTokenCalled);
            handler.GetCalled = false;
            handler.GetAccessTokenCalled = false;
            handler.SendCalled = false;

            // Create
            Assert.True(!handler.CreateCalled);
            client.Create(dto);
            Assert.True(handler.CreateCalled);
            Assert.True(handler.SendCalled);
            Assert.True(!handler.GetAccessTokenCalled); // Cached
            handler.CreateCalled = false;
            handler.GetAccessTokenCalled = false;
            handler.SendCalled = false;

            // CreateAck
            Assert.True(!handler.CreateAckCalled);
            client.CreateAck(dto);
            Assert.True(handler.CreateAckCalled);
            Assert.True(handler.SendCalled);
            Assert.True(!handler.GetAccessTokenCalled); // Cached
            handler.CreateAckCalled = false;
            handler.GetAccessTokenCalled = false;
            handler.SendCalled = false;

            // Update
            Assert.True(!handler.UpdateCalled);
            client.Update(dto);
            Assert.True(handler.UpdateCalled);
            Assert.True(handler.SendCalled);
            Assert.True(!handler.GetAccessTokenCalled); // Cached
            handler.UpdateCalled = false;
            handler.GetAccessTokenCalled = false;
            handler.SendCalled = false;

            // UpdateAck
            Assert.True(!handler.UpdateAckCalled);
            client.UpdateAck(dto);
            Assert.True(handler.UpdateAckCalled);
            Assert.True(handler.SendCalled);
            Assert.True(!handler.GetAccessTokenCalled); // Cached
            handler.UpdateAckCalled = false;
            handler.GetAccessTokenCalled = false;
            handler.SendCalled = false;

            // Delete
            Assert.True(!handler.DeleteCalled);
            client.Delete(dto.StorageKey);
            Assert.True(handler.DeleteCalled);
            Assert.True(handler.SendCalled);
            Assert.True(!handler.GetAccessTokenCalled); // Cached
            handler.DeleteCalled = false;
            handler.GetAccessTokenCalled = false;
            handler.SendCalled = false;

            // Query
            Assert.True(!handler.QueryCalled);
            client.Query(new ServiceQueryRequest());
            Assert.True(handler.QueryCalled);
            Assert.True(handler.SendCalled);
            Assert.True(!handler.GetAccessTokenCalled); // Cached
            handler.QueryCalled = false;
            handler.GetAccessTokenCalled = false;
            handler.SendCalled = false;

            // Patch
            Assert.True(!handler.PatchCalled);
            client.Patch(dto.StorageKey, new Microsoft.AspNetCore.JsonPatch.JsonPatchDocument<ExampleDto>());
            Assert.True(handler.PatchCalled);
            Assert.True(handler.SendCalled);
            Assert.True(!handler.GetAccessTokenCalled); // Cached
            handler.PatchCalled = false;
            handler.GetAccessTokenCalled = false;
            handler.SendCalled = false;

            // PatchAck
            Assert.True(!handler.PatchAckCalled);
            client.PatchAck(dto.StorageKey, new Microsoft.AspNetCore.JsonPatch.JsonPatchDocument<ExampleDto>());
            Assert.True(handler.PatchAckCalled);
            Assert.True(handler.SendCalled);
            Assert.True(!handler.GetAccessTokenCalled); // Cached
            handler.PatchAckCalled = false;
            handler.GetAccessTokenCalled = false;
            handler.SendCalled = false;

            // Validate
            Assert.True(!handler.ValidateCalled);
            client.Validate(dto);
            Assert.True(handler.ValidateCalled);
            Assert.True(handler.SendCalled);
            Assert.True(!handler.GetAccessTokenCalled); // Cached
            handler.ValidateCalled = false;
            handler.GetAccessTokenCalled = false;
            handler.SendCalled = false;

            return Task.CompletedTask;
        }

        [Fact]
        public virtual async Task ApiClientCallWithAuthSuccessAsync()
        {
            var loggerFactory = SystemManager.ServiceProvider.GetRequiredService<ILoggerFactory>();
            var options = new ClientApiOptions()
            {
                BaseServiceUrl = "https://localhost:7000/api/v1.0",
                DisableAuthentication = false,
                ReturnResponseObject = false,
                TokenUrl = "https://localhost:7000/api/v1.0/token",
                TokenClient = "ApiClientCallWithAuthSuccessAsync"
            };
            var handler = new CustomGenericHttpClientHandler<ExampleDto>(null);
            var handlerFactory = new ExampleHttpClientFactory(handler);
            var client = new ExampleApiClient(
                loggerFactory,
                handlerFactory,
                options);

            var dto = new ExampleDto() { StorageKey = "1" };

            // Reset
            handler.SendAsyncCalled = false;
            handler.GetAccessTokenAsyncCalled = false;

            // Get
            Assert.True(!handler.GetAsyncCalled);
            await client.GetAsync(dto.StorageKey);
            Assert.True(handler.SendAsyncCalled);
            Assert.True(handler.GetAccessTokenAsyncCalled);
            handler.SendAsyncCalled = false;
            handler.GetAccessTokenAsyncCalled = false;

            // Create
            Assert.True(!handler.CreateAsyncCalled);
            await client.CreateAsync(dto);
            Assert.True(handler.CreateAsyncCalled);
            Assert.True(handler.SendAsyncCalled);
            Assert.True(!handler.GetAccessTokenAsyncCalled); // Cached
            handler.CreateAsyncCalled = false;
            handler.SendAsyncCalled = false;

            // CreateAck
            Assert.True(!handler.CreateAckAsyncCalled);
            await client.CreateAckAsync(dto);
            Assert.True(handler.CreateAckAsyncCalled);
            Assert.True(handler.SendAsyncCalled);
            Assert.True(!handler.GetAccessTokenAsyncCalled); // Cached
            handler.CreateAckAsyncCalled = false;
            handler.SendAsyncCalled = false;

            // Update
            Assert.True(!handler.UpdateAsyncCalled);
            await client.UpdateAsync(dto);
            Assert.True(handler.UpdateAsyncCalled);
            Assert.True(handler.SendAsyncCalled);
            Assert.True(!handler.GetAccessTokenAsyncCalled); // Cached
            handler.UpdateAsyncCalled = false;
            handler.SendAsyncCalled = false;

            // UpdateAck
            Assert.True(!handler.UpdateAsyncCalled);
            await client.UpdateAckAsync(dto);
            Assert.True(handler.SendAsyncCalled);
            Assert.True(handler.UpdateAckAsyncCalled);
            Assert.True(!handler.GetAccessTokenAsyncCalled); // Cached
            handler.UpdateAckAsyncCalled = false;
            handler.SendAsyncCalled = false;

            // Delete
            Assert.True(!handler.DeleteAsyncCalled);
            await client.DeleteAsync(dto.StorageKey);
            Assert.True(handler.SendAsyncCalled);
            Assert.True(handler.DeleteAsyncCalled);
            Assert.True(!handler.GetAccessTokenAsyncCalled); // Cached
            handler.DeleteAsyncCalled = false;
            handler.SendAsyncCalled = false;

            // Query
            Assert.True(!handler.QueryAsyncCalled);
            await client.QueryAsync(new ServiceQueryRequest());
            Assert.True(handler.SendAsyncCalled);
            Assert.True(handler.QueryAsyncCalled);
            Assert.True(!handler.GetAccessTokenAsyncCalled); // Cached
            handler.QueryAsyncCalled = false;
            handler.SendAsyncCalled = false;

            // Patch
            Assert.True(!handler.PatchAsyncCalled);
            await client.PatchAsync(dto.StorageKey, new Microsoft.AspNetCore.JsonPatch.JsonPatchDocument<ExampleDto>());
            Assert.True(handler.SendAsyncCalled);
            Assert.True(handler.PatchAsyncCalled);
            Assert.True(!handler.GetAccessTokenAsyncCalled); // Cached
            handler.PatchAsyncCalled = false;
            handler.SendAsyncCalled = false;

            // PatchAck
            Assert.True(!handler.PatchAckAsyncCalled);
            await client.PatchAckAsync(dto.StorageKey, new Microsoft.AspNetCore.JsonPatch.JsonPatchDocument<ExampleDto>());
            Assert.True(handler.SendAsyncCalled);
            Assert.True(handler.PatchAckAsyncCalled);
            Assert.True(!handler.GetAccessTokenAsyncCalled); // Cached
            handler.PatchAckAsyncCalled = false;

            // Validate
            Assert.True(!handler.ValidateAsyncCalled);
            await client.ValidateAsync(dto);
            Assert.True(handler.SendAsyncCalled);
            Assert.True(handler.ValidateAsyncCalled);
            Assert.True(!handler.GetAccessTokenAsyncCalled); // Cached
            handler.ValidateAsyncCalled = false;
        }

        [Fact]
        public virtual Task ApiClientCallNullSendError()
        {
            var loggerFactory = SystemManager.ServiceProvider.GetRequiredService<ILoggerFactory>();
            var options = new ClientApiOptions()
            {
                BaseServiceUrl = "https://localhost:7000/api/v1.0",
                DisableAuthentication = true,
                ReturnResponseObject = false,
                TokenUrl = "https://localhost:7000/api/v1.0/token",
                TokenClient = "ApiClientCallNullSendError"
            };
            var handler = new ReturnNullHttpClientHandler();
            var handlerFactory = new ExampleHttpClientFactory(handler);
            var client = new ExampleApiClient(
                loggerFactory,
                handlerFactory,
                options);

            var dto = new ExampleDto() { StorageKey = "1" };

            // Get
            var respGet = client.Get(dto.StorageKey);
            Assert.True(respGet.Error);

            // Create
            var respCreate = client.Create(dto);
            Assert.True(respCreate.Error);

            // Create
            var respCreateAck = client.CreateAck(dto);
            Assert.True(respCreateAck.Error);

            // Update
            var respUpdate = client.Update(dto);
            Assert.True(respUpdate.Error);

            // UpdateAck
            var respUpdateAck = client.UpdateAck(dto);
            Assert.True(respUpdateAck.Error);

            // Delete
            var respDelete = client.Delete(dto.StorageKey);
            Assert.True(respDelete.Error);

            // Query
            var respQuery = client.Query(new ServiceQueryRequest());
            Assert.True(respQuery.Error);

            // Patch
            var respPatch = client.Patch(dto.StorageKey, new Microsoft.AspNetCore.JsonPatch.JsonPatchDocument<ExampleDto>());
            Assert.True(respPatch.Error);

            // PatchAck
            var respPatchAck = client.PatchAck(dto.StorageKey, new Microsoft.AspNetCore.JsonPatch.JsonPatchDocument<ExampleDto>());
            Assert.True(respPatchAck.Error);

            // Validate
            var respValidate = client.Validate(dto);
            Assert.True(respValidate.Error);

            return Task.CompletedTask;
        }

        [Fact]
        public virtual async Task ApiClientCallNullSendErrorAsync()
        {
            var loggerFactory = SystemManager.ServiceProvider.GetRequiredService<ILoggerFactory>();
            var options = new ClientApiOptions()
            {
                BaseServiceUrl = "https://localhost:7000/api/v1.0",
                DisableAuthentication = true,
                ReturnResponseObject = false,
                TokenUrl = "https://localhost:7000/api/v1.0/token",
            };
            var handler = new ReturnNullHttpClientHandler();
            var handlerFactory = new ExampleHttpClientFactory(handler);
            var client = new ExampleApiClient(
                loggerFactory,
                handlerFactory,
                options);

            var dto = new ExampleDto() { StorageKey = "1" };

            // Get
            var respGet = await client.GetAsync(dto.StorageKey);
            Assert.True(respGet.Error);

            // Create
            var respCreate = await client.CreateAsync(dto);
            Assert.True(respCreate.Error);

            // Create
            var respCreateAck = await client.CreateAckAsync(dto);
            Assert.True(respCreateAck.Error);

            // Update
            var respUpdate = await client.UpdateAsync(dto);
            Assert.True(respUpdate.Error);

            // UpdateAck
            var respUpdateAck = await client.UpdateAckAsync(dto);
            Assert.True(respUpdateAck.Error);

            // Delete
            var respDelete = await client.DeleteAsync(dto.StorageKey);
            Assert.True(respDelete.Error);

            // Query
            var respQuery = await client.QueryAsync(new ServiceQueryRequest());
            Assert.True(respQuery.Error);

            // Patch
            var respPatch = await client.PatchAsync(dto.StorageKey, new Microsoft.AspNetCore.JsonPatch.JsonPatchDocument<ExampleDto>());
            Assert.True(respPatch.Error);

            // PatchAck
            var respPatchAck = await client.PatchAckAsync(dto.StorageKey, new Microsoft.AspNetCore.JsonPatch.JsonPatchDocument<ExampleDto>());
            Assert.True(respPatchAck.Error);

            // Validate
            var respValidate = await client.ValidateAsync(dto);
            Assert.True(respValidate.Error);
        }
    }
}