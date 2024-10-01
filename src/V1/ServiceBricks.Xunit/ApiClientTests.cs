using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ServiceQuery;

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

        public class CustomGenericHttpClientHandler<TDto> : HttpClientHandler where TDto : IDataTransferObject
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

            protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                SendCalled = true;

                string url = request.RequestUri.ToString();
                string content = string.Empty;
                if (request.Content != null)
                    content = request.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                if (request.Method == HttpMethod.Get)
                {
                    GetCalled = true;
                    if (_service != null && url.Contains("?storageKey=", StringComparison.InvariantCultureIgnoreCase))
                    {
                        var storageKey = url.Substring(url.IndexOf("?storageKey=", StringComparison.InvariantCultureIgnoreCase) + 12);
                        var resp = _service.Get(storageKey);
                        return GetClientResponse(resp);
                    }
                }
                else if (request.Method == HttpMethod.Put)
                {
                    UpdateCalled = true;

                    if (_service != null)
                    {
                        var dto = JsonConvert.DeserializeObject<TDto>(content);
                        var resp = _service.Update(dto);
                        return GetClientResponse(resp);
                    }
                }
                else if (request.Method == HttpMethod.Delete)
                {
                    DeleteCalled = true;
                    if (_service != null && url.Contains("?storageKey=", StringComparison.InvariantCultureIgnoreCase))
                    {
                        var storageKey = url.Substring(url.IndexOf("?storageKey=", StringComparison.InvariantCultureIgnoreCase) + 12);
                        var resp = _service.Delete(storageKey);
                        return GetClientResponse(resp);
                    }
                }
                else if (request.Method == HttpMethod.Post)
                {
                    if (url.Contains("query", StringComparison.InvariantCultureIgnoreCase))
                    {
                        QueryCalled = true;
                        if (_service != null)
                        {
                            var dto = JsonConvert.DeserializeObject<ServiceQueryRequest>(content);
                            var resp = _service.Query(dto);
                            return GetClientResponse(resp);
                        }
                    }
                    if (url.Contains("create", StringComparison.InvariantCultureIgnoreCase))
                    {
                        CreateCalled = true;
                        if (_service != null)
                        {
                            var dto = JsonConvert.DeserializeObject<TDto>(content);
                            var resp = _service.Create(dto);
                            return GetClientResponse(resp);
                        }
                    }
                    else if (url.Contains("token", StringComparison.InvariantCultureIgnoreCase))
                    {
                        GetAccessTokenCalled = true;

                        AccessTokenResponse resp = new AccessTokenResponse();
                        resp.access_token = Guid.NewGuid().ToString();
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
                    return new HttpResponseMessage()
                    {
                        Content = new StringContent(JsonConvert.SerializeObject(or.Value)),
                        StatusCode = or.StatusCode.Value == 200 ? System.Net.HttpStatusCode.OK : System.Net.HttpStatusCode.BadRequest
                    };
                }
                return new HttpResponseMessage()
                {
                    Content = new StringContent(JsonConvert.SerializeObject(response))
                };
            }

            public bool GetAccessTokenAsyncCalled { get; set; }
            public bool SendAsyncCalled { get; set; }
            public bool CreateAsyncCalled { get; set; }
            public bool GetAsyncCalled { get; set; }
            public bool UpdateAsyncCalled { get; set; }
            public bool DeleteAsyncCalled { get; set; }
            public bool QueryAsyncCalled { get; set; }

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
                    if (_service != null && url.Contains("?storageKey=", StringComparison.InvariantCultureIgnoreCase))
                    {
                        var storageKey = url.Substring(url.IndexOf("?storageKey=", StringComparison.InvariantCultureIgnoreCase) + 12);
                        var resp = _service.Get(storageKey);
                        return GetClientResponse(resp);
                    }
                }
                else if (request.Method == HttpMethod.Put)
                {
                    UpdateAsyncCalled = true;

                    if (_service != null)
                    {
                        var dto = JsonConvert.DeserializeObject<TDto>(content);
                        var resp = _service.Update(dto);
                        return GetClientResponse(resp);
                    }
                }
                else if (request.Method == HttpMethod.Delete)
                {
                    DeleteAsyncCalled = true;
                    if (_service != null && url.Contains("?storageKey=", StringComparison.InvariantCultureIgnoreCase))
                    {
                        var storageKey = url.Substring(url.IndexOf("?storageKey=", StringComparison.InvariantCultureIgnoreCase) + 12);
                        var resp = _service.Delete(storageKey);
                        return GetClientResponse(resp);
                    }
                }
                else if (request.Method == HttpMethod.Post)
                {
                    if (url.Contains("query", StringComparison.InvariantCultureIgnoreCase))
                    {
                        QueryAsyncCalled = true;
                        if (_service != null)
                        {
                            var dto = JsonConvert.DeserializeObject<ServiceQueryRequest>(content);
                            var resp = _service.Query(dto);
                            return GetClientResponse(resp);
                        }
                    }
                    if (url.Contains("create", StringComparison.InvariantCultureIgnoreCase))
                    {
                        CreateAsyncCalled = true;
                        if (_service != null)
                        {
                            var dto = JsonConvert.DeserializeObject<TDto>(content);
                            var resp = _service.Create(dto);
                            return GetClientResponse(resp);
                        }
                    }
                    else if (url.Contains("token", StringComparison.InvariantCultureIgnoreCase))
                    {
                        GetAccessTokenAsyncCalled = true;

                        AccessTokenResponse resp = new AccessTokenResponse();
                        resp.access_token = Guid.NewGuid().ToString();
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
            client.Get(dto.StorageKey);
            Assert.True(handler.SendCalled);
            Assert.True(!handler.GetAccessTokenCalled);
            handler.SendCalled = false;
            handler.CreateCalled = false;

            // Create
            Assert.True(!handler.CreateCalled);
            client.Create(dto);
            Assert.True(handler.CreateCalled);
            Assert.True(!handler.GetAccessTokenCalled);
            handler.CreateCalled = false;

            // Update
            Assert.True(!handler.UpdateCalled);
            client.Update(dto);
            Assert.True(handler.UpdateCalled);
            Assert.True(!handler.GetAccessTokenCalled);
            handler.UpdateCalled = false;

            // Delete
            Assert.True(!handler.DeleteCalled);
            client.Delete(dto.StorageKey);
            Assert.True(handler.SendCalled);
            Assert.True(!handler.GetAccessTokenCalled);
            handler.DeleteCalled = false;

            // Query
            Assert.True(!handler.QueryCalled);
            client.Query(new ServiceQueryRequest());
            Assert.True(handler.SendCalled);
            Assert.True(!handler.GetAccessTokenCalled);
            handler.QueryCalled = false;

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
            Assert.True(!handler.GetAccessTokenAsyncCalled);
            handler.SendAsyncCalled = false;
            handler.CreateAsyncCalled = false;

            // Create
            Assert.True(!handler.CreateAsyncCalled);
            await client.CreateAsync(dto);
            Assert.True(handler.CreateAsyncCalled);
            Assert.True(!handler.GetAccessTokenAsyncCalled);
            handler.CreateAsyncCalled = false;

            // Update
            Assert.True(!handler.UpdateAsyncCalled);
            await client.UpdateAsync(dto);
            Assert.True(handler.UpdateAsyncCalled);
            Assert.True(!handler.GetAccessTokenAsyncCalled);
            handler.UpdateAsyncCalled = false;

            // Delete
            Assert.True(!handler.DeleteAsyncCalled);
            await client.DeleteAsync(dto.StorageKey);
            Assert.True(handler.SendAsyncCalled);
            Assert.True(!handler.GetAccessTokenAsyncCalled);
            handler.DeleteAsyncCalled = false;

            // Query
            Assert.True(!handler.QueryAsyncCalled);
            await client.QueryAsync(new ServiceQueryRequest());
            Assert.True(handler.SendAsyncCalled);
            Assert.True(!handler.GetAccessTokenAsyncCalled);
            handler.QueryAsyncCalled = false;
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
            client.Get(dto.StorageKey);
            Assert.True(handler.SendCalled);
            Assert.True(handler.GetAccessTokenCalled);
            handler.SendCalled = false;
            handler.CreateCalled = false;

            // Create
            Assert.True(!handler.CreateCalled);
            client.Create(dto);
            Assert.True(handler.CreateCalled);
            Assert.True(handler.GetAccessTokenCalled);
            handler.CreateCalled = false;

            // Update
            Assert.True(!handler.UpdateCalled);
            client.Update(dto);
            Assert.True(handler.UpdateCalled);
            Assert.True(handler.GetAccessTokenCalled);
            handler.UpdateCalled = false;

            // Delete
            Assert.True(!handler.DeleteCalled);
            client.Delete(dto.StorageKey);
            Assert.True(handler.SendCalled);
            Assert.True(handler.GetAccessTokenCalled);
            handler.DeleteCalled = false;

            // Query
            Assert.True(!handler.QueryCalled);
            client.Query(new ServiceQueryRequest());
            Assert.True(handler.SendCalled);
            Assert.True(handler.GetAccessTokenCalled);
            handler.QueryCalled = false;

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
            Assert.True(handler.GetAccessTokenAsyncCalled);
            handler.SendAsyncCalled = false;
            handler.CreateAsyncCalled = false;

            // Create
            Assert.True(!handler.CreateAsyncCalled);
            await client.CreateAsync(dto);
            Assert.True(handler.CreateAsyncCalled);
            Assert.True(handler.GetAccessTokenAsyncCalled);
            handler.CreateAsyncCalled = false;

            // Update
            Assert.True(!handler.UpdateAsyncCalled);
            await client.UpdateAsync(dto);
            Assert.True(handler.UpdateAsyncCalled);
            Assert.True(handler.GetAccessTokenAsyncCalled);
            handler.UpdateAsyncCalled = false;

            // Delete
            Assert.True(!handler.DeleteAsyncCalled);
            await client.DeleteAsync(dto.StorageKey);
            Assert.True(handler.SendAsyncCalled);
            Assert.True(handler.GetAccessTokenAsyncCalled);
            handler.DeleteAsyncCalled = false;

            // Query
            Assert.True(!handler.QueryAsyncCalled);
            await client.QueryAsync(new ServiceQueryRequest());
            Assert.True(handler.SendAsyncCalled);
            Assert.True(handler.GetAccessTokenAsyncCalled);
            handler.QueryAsyncCalled = false;
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

            // Update
            var respUpdate = client.Update(dto);
            Assert.True(respUpdate.Error);

            // Delete
            var respDelete = client.Delete(dto.StorageKey);
            Assert.True(respDelete.Error);

            // Query
            var respQuery = client.Query(new ServiceQueryRequest());
            Assert.True(respQuery.Error);

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

            // Update
            var respUpdate = await client.UpdateAsync(dto);
            Assert.True(respUpdate.Error);

            // Delete
            var respDelete = await client.DeleteAsync(dto.StorageKey);
            Assert.True(respDelete.Error);

            // Query
            var respQuery = await client.QueryAsync(new ServiceQueryRequest());
            Assert.True(respQuery.Error);
        }
    }
}