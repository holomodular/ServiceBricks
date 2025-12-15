using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.TestDataTypes;

namespace ServiceBricks.Xunit.Integration
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public class TestApiControllerTestSqlServer : TestApiControllerTest
    {
        public TestApiControllerTestSqlServer()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(StartupSqlServer));
            TestManager = SystemManager.ServiceProvider.GetRequiredService<ITestManager<TestDto>>();
        }
    }
}
