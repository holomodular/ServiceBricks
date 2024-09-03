using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ServiceQuery;

namespace ServiceBricks.Xunit
{
    [Collection(Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public partial class ExtensionTests
    {
        public virtual ISystemManager SystemManager { get; set; }

        public ExtensionTests()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(ServiceBricksStartup));
        }

        public class TestHttpContextAccessor : IHttpContextAccessor
        {
            public TestHttpContextAccessor()
            {
                HttpContext = new DefaultHttpContext();
            }

            public HttpContext HttpContext { get; set; }
        }

        [Fact]
        public virtual Task ConfigurationBuilderTests()
        {
            ConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddAppSettingsConfig();

            var config = builder.Build();

            var count = config.Providers.Count();
            Assert.True(count > 0);

            return Task.CompletedTask;
        }

        [Fact]
        public virtual Task ApiConfigTests()
        {
            ConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddAppSettingsConfig();
            var config = builder.Build();

            var apiconfig = config.GetApiConfig();
            Assert.True(apiconfig != null);

            return Task.CompletedTask;
        }

        [Fact]
        public virtual Task GetUserStorageKeyTests()
        {
            TestHttpContextAccessor contextAccessor = new TestHttpContextAccessor();
            ConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddAppSettingsConfig();
            var config = builder.Build();

            var uk = contextAccessor.GetUserStorageKey();
            var userkey = contextAccessor.HttpContext.GetUserStorageKey();

            return Task.CompletedTask;
        }

        [Fact]
        public virtual Task ModelStateTests()
        {
            ModelStateDictionary modelstate = new ModelStateDictionary();

            modelstate.AddModelError("key", "error");

            var response = new Response();
            modelstate.CopyToResponse(response);
            Assert.True(response.Error);

            response = new Response();
            modelstate = new ModelStateDictionary();
            response.AddMessage(ResponseMessage.CreateError("error"));
            modelstate.CopyFromResponse(response);
            Assert.True(modelstate.ErrorCount > 0);

            return Task.CompletedTask;
        }
    }
}