using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ServiceBricks.Xunit
{
    [Collection(Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public partial class MiddlewareTests
    {
        public virtual ISystemManager SystemManager { get; set; }

        public MiddlewareTests()
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
        public virtual async Task PropogateExceptionTest()
        {
            var testHttpContextAccessor = new TestHttpContextAccessor();
            testHttpContextAccessor.HttpContext.Request.Path = "/api/test";
            var loggerFactory = SystemManager.ServiceProvider.GetRequiredService<ILoggerFactory>();
            var mapper = SystemManager.ServiceProvider.GetRequiredService<IMapper>();
            var options = new OptionsWrapper<ApiOptions>(new ApiOptions() { });

            Exception exception = null;
            try
            {
                PropogateExceptionResponseMiddleware middleware = new PropogateExceptionResponseMiddleware(
                                    (context) =>
                                   {
                                       throw new Exception("Test Exception");
                                   },
                                 loggerFactory,
                                 mapper,
                                 options);

                await middleware.InvokeAsync(testHttpContextAccessor.HttpContext);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            Assert.True(exception != null);
        }

        [Fact]
        public virtual async Task PropogateExceptionTestReturnResponse()
        {
            var testHttpContextAccessor = new TestHttpContextAccessor();
            testHttpContextAccessor.HttpContext.Request.Path = "/api/test";
            var loggerFactory = SystemManager.ServiceProvider.GetRequiredService<ILoggerFactory>();
            var mapper = SystemManager.ServiceProvider.GetRequiredService<IMapper>();
            var options = new OptionsWrapper<ApiOptions>(new ApiOptions() { ReturnResponseObject = true });

            Exception exception = null;
            try
            {
                PropogateExceptionResponseMiddleware middleware = new PropogateExceptionResponseMiddleware(
                                    (context) =>
                                    {
                                        throw new Exception("Test Exception");
                                    },
                                 loggerFactory,
                                 mapper,
                                 options);

                await middleware.InvokeAsync(testHttpContextAccessor.HttpContext);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            Assert.True(exception != null);
        }

        [Fact]
        public virtual async Task TrapExceptionTest()
        {
            var testHttpContextAccessor = new TestHttpContextAccessor();
            testHttpContextAccessor.HttpContext.Request.Path = "/api/test";
            var loggerFactory = SystemManager.ServiceProvider.GetRequiredService<ILoggerFactory>();
            var mapper = SystemManager.ServiceProvider.GetRequiredService<IMapper>();
            var options = new OptionsWrapper<ApiOptions>(new ApiOptions() { });

            Exception exception = null;
            try
            {
                TrapExceptionResponseMiddleware middleware = new TrapExceptionResponseMiddleware(
                                   (context) =>
                                   {
                                       throw new Exception("Test Exception");
                                   },
                                 loggerFactory,
                                 mapper,
                                 options);

                await middleware.InvokeAsync(testHttpContextAccessor.HttpContext);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            Assert.True(exception == null);
        }

        [Fact]
        public virtual async Task TrapExceptionTestReturnResponse()
        {
            var testHttpContextAccessor = new TestHttpContextAccessor();
            testHttpContextAccessor.HttpContext.Request.Path = "/api/test";
            var loggerFactory = SystemManager.ServiceProvider.GetRequiredService<ILoggerFactory>();
            var mapper = SystemManager.ServiceProvider.GetRequiredService<IMapper>();
            var options = new OptionsWrapper<ApiOptions>(new ApiOptions() { ReturnResponseObject = true });

            Exception exception = null;
            try
            {
                TrapExceptionResponseMiddleware middleware = new TrapExceptionResponseMiddleware(
                                   (context) =>
                                   {
                                       throw new Exception("Test Exception");
                                   },
                                 loggerFactory,
                                 mapper,
                                 options);

                await middleware.InvokeAsync(testHttpContextAccessor.HttpContext);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            Assert.True(exception == null);
        }
    }
}