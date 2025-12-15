using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.TestDataTypes;

namespace ServiceBricks.Xunit.Integration
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public class TestApiControllerTestMongoDb : TestApiControllerTest
    {
        public TestApiControllerTestMongoDb()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(StartupMongoDb));
            TestManager = SystemManager.ServiceProvider.GetRequiredService<ITestManager<TestDto>>();
        }
    }
}
