using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ServiceQuery;
using System.Runtime.InteropServices;
using System.Text;

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

        public class CustomHttpClientHandler : HttpClientHandler
        {
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

                if (request.Method == HttpMethod.Get)
                {
                    GetCalled = true;
                }
                else if (request.Method == HttpMethod.Put)
                {
                    UpdateCalled = true;
                }
                else if (request.Method == HttpMethod.Delete)
                {
                    DeleteCalled = true;
                }
                else if (request.Method == HttpMethod.Post)
                {
                    if (request.RequestUri.PathAndQuery.Contains("query", StringComparison.InvariantCultureIgnoreCase))
                        QueryCalled = true;
                    if (request.RequestUri.PathAndQuery.Contains("create", StringComparison.InvariantCultureIgnoreCase))
                        CreateCalled = true;
                    else if (request.RequestUri.PathAndQuery.Contains("token", StringComparison.InvariantCultureIgnoreCase))
                        GetAccessTokenCalled = true;
                }
                return new HttpResponseMessage();
            }

            public bool GetAccessTokenAsyncCalled { get; set; }
            public bool SendAsyncCalled { get; set; }
            public bool CreateAsyncCalled { get; set; }
            public bool GetAsyncCalled { get; set; }
            public bool UpdateAsyncCalled { get; set; }
            public bool DeleteAsyncCalled { get; set; }
            public bool QueryAsyncCalled { get; set; }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                SendAsyncCalled = true;

                if (request.Method == HttpMethod.Get)
                {
                    GetAsyncCalled = true;
                }
                else if (request.Method == HttpMethod.Put)
                {
                    UpdateAsyncCalled = true;
                }
                else if (request.Method == HttpMethod.Delete)
                {
                    DeleteAsyncCalled = true;
                }
                else if (request.Method == HttpMethod.Post)
                {
                    if (request.RequestUri.PathAndQuery.Contains("query", StringComparison.InvariantCultureIgnoreCase))
                        QueryAsyncCalled = true;
                    else if (request.RequestUri.PathAndQuery.Contains("create", StringComparison.InvariantCultureIgnoreCase))
                        CreateAsyncCalled = true;
                    else if (request.RequestUri.PathAndQuery.Contains("token", StringComparison.InvariantCultureIgnoreCase))
                        GetAccessTokenAsyncCalled = true;
                }
                return Task.FromResult(new HttpResponseMessage());
            }
        }

        public class ExampleHttpClientFactory : IHttpClientFactory
        {
            private CustomHttpClientHandler _handler;

            public ExampleHttpClientFactory(CustomHttpClientHandler handler)
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
            var handler = new CustomHttpClientHandler();
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
            var handler = new CustomHttpClientHandler();
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
            var handler = new CustomHttpClientHandler();
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
            handler.GetAccessTokenCalled = false;

            // Create
            Assert.True(!handler.CreateCalled);
            client.Create(dto);
            Assert.True(handler.CreateCalled);
            Assert.True(handler.GetAccessTokenCalled);
            handler.CreateCalled = false;
            handler.GetAccessTokenCalled = false;

            // Update
            Assert.True(!handler.UpdateCalled);
            client.Update(dto);
            Assert.True(handler.UpdateCalled);
            Assert.True(handler.GetAccessTokenCalled);
            handler.UpdateCalled = false;
            handler.GetAccessTokenCalled = false;

            // Delete
            Assert.True(!handler.DeleteCalled);
            client.Delete(dto.StorageKey);
            Assert.True(handler.SendCalled);
            Assert.True(handler.GetAccessTokenCalled);
            handler.DeleteCalled = false;
            handler.GetAccessTokenCalled = false;

            // Query
            Assert.True(!handler.QueryCalled);
            client.Query(new ServiceQueryRequest());
            Assert.True(handler.SendCalled);
            Assert.True(handler.GetAccessTokenCalled);
            handler.QueryCalled = false;
            handler.GetAccessTokenCalled = false;

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
            var handler = new CustomHttpClientHandler();
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
    }
}