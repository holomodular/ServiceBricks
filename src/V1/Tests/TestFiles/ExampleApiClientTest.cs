using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Xunit;
using ServiceBricks.Client.Xunit;

namespace ServiceBricks.Xunit.Integration
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public class ExampleApiClientTest : ApiClientTest<ExampleDto>
    {
        public ExampleApiClientTest()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(ClientStartup));
            TestManager = SystemManager.ServiceProvider.GetRequiredService<ITestManager<ExampleDto>>();
        }
    }
}