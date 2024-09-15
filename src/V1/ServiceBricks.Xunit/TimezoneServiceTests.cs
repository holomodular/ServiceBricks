using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ServiceQuery;
using static ServiceBricks.Xunit.IpAddressServiceTests;

namespace ServiceBricks.Xunit
{
    [Collection(Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public partial class TimezoneServiceTests
    {
        public virtual ISystemManager SystemManager { get; set; }

        public TimezoneServiceTests()
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
        public virtual Task TimezoneServiceSuccess()
        {
            var httpContextAccessor = new TestHttpContextAccessor();
            var timezoneservice = new TimezoneService(httpContextAccessor);

            // Get all timezones
            var respTimezones = timezoneservice.GetTimezones();
            Assert.True(respTimezones.Count > 0);

            // Get local timezone
            var defaulttimezone = timezoneservice.GetDefaultTimezoneId();
            Assert.True(!string.IsNullOrEmpty(defaulttimezone));

            // Get User Timezone
            var info = timezoneservice.GetUserTimezoneInfo();
            Assert.True(info != null);
            string curid = info.Id;

            // Get Default Timezone
            var tname = TimezoneService.DefaultTimezoneName();
            Assert.True(tname == defaulttimezone);

            // Change timezone
            timezoneservice.ChangeUserTimezone("Central Standard Time");

            // Get User Timezone
            info = timezoneservice.GetUserTimezoneInfo();
            Assert.True(info.Id != curid);

            // setup testdate
            var testdatezero = new DateTimeOffset(2000, 1, 1, 1, 1, 1, TimeSpan.Zero);
            var testlocaloffset = info.GetUtcOffset(testdatezero);
            var testlocaldate = new DateTimeOffset(2000, 1, 1, 1, 1, 1, testlocaloffset);

            var localnow = DateTimeOffset.Now;
            var globalnow = DateTimeOffset.UtcNow;

            // Convert for user
            var converttoutc = timezoneservice.ConvertLocalToUTCForUser(testlocaldate);
            Assert.True(converttoutc.Offset == TimeSpan.Zero);
            Assert.True(converttoutc == testlocaldate);

            // Convert for server
            var convertpost = timezoneservice.ConvertPostBackToUTC(localnow);
            Assert.True(converttoutc.Offset == TimeSpan.Zero);

            // Convert for local user
            var usertime = timezoneservice.ConvertUtcToLocalForUser(globalnow);
            Assert.True(usertime.Offset == info.GetUtcOffset(globalnow));

            // Convert to local
            var localtime = timezoneservice.ConvertUtcToLocal(globalnow, info.Id);
            Assert.True(usertime.Offset == info.GetUtcOffset(globalnow));
            Assert.True(localtime.Offset == info.GetUtcOffset(globalnow));

            return Task.CompletedTask;
        }
    }
}