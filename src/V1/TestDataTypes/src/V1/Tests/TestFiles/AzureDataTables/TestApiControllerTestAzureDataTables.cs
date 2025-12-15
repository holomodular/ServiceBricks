
using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.TestDataTypes;

namespace ServiceBricks.Xunit.Integration
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public class TestApiControllerTestAzureDataTables : TestApiControllerTest
    {
        public TestApiControllerTestAzureDataTables()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(StartupAzureDataTables));
            TestManager = SystemManager.ServiceProvider.GetRequiredService<ITestManager<TestDto>>();
        }
    }
}
