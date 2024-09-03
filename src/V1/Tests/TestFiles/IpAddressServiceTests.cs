using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ServiceQuery;

namespace ServiceBricks.Xunit
{
    [Collection(Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public partial class IpAddressServiceTests
    {
        public virtual ISystemManager SystemManager { get; set; }

        public IpAddressServiceTests()
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
        public virtual Task IPAddressServiceSuccess()
        {
            var httpContextAccessor = new TestHttpContextAccessor();
            IpAddressService ipAddressService = new IpAddressService(httpContextAccessor);

            var resp = ipAddressService.GetIPAddress();

            var respAddress = IpAddressService.GetIPAddress(httpContextAccessor.HttpContext);

            return Task.CompletedTask;
        }
    }
}