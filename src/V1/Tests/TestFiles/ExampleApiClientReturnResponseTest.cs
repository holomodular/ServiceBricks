using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic;
using ServiceBricks.Client.Xunit;

namespace ServiceBricks.Xunit.Integration
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public class ExampleApiClientReturnResponseTest : ApiClientReturnResponseTest<ExampleDto>
    {
        public ExampleApiClientReturnResponseTest()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(ClientStartup));
            TestManager = SystemManager.ServiceProvider.GetRequiredService<ITestManager<ExampleDto>>();
        }
    }
}