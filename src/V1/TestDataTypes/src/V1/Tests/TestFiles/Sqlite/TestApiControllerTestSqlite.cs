using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.TestDataTypes;

namespace ServiceBricks.Xunit.Integration
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public class TestApiControllerTestSqlite : TestApiControllerTest
    {
        public TestApiControllerTestSqlite()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(StartupSqlite));
            TestManager = SystemManager.ServiceProvider.GetRequiredService<ITestManager<TestDto>>();
        }
    }
}
