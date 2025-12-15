using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.TestDataTypes;
using ServiceBricks.TestDataTypes.Client.Xunit;

namespace ServiceBricks.Xunit.Integration
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public class TestApiClientTest : ApiClientTest<TestDto>
    {
        public TestApiClientTest()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(ClientStartup));
            TestManager = SystemManager.ServiceProvider.GetRequiredService<ITestManager<TestDto>>();
        }
    }
}
