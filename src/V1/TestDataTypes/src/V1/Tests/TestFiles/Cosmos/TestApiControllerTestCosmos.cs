using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.TestDataTypes;

namespace ServiceBricks.Xunit.Integration
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public class TestApiControllerTestCosmos : TestApiControllerTest
    {
        public TestApiControllerTestCosmos()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(StartupCosmos));
            TestManager = SystemManager.ServiceProvider.GetRequiredService<ITestManager<TestDto>>();
        }
    }
}
