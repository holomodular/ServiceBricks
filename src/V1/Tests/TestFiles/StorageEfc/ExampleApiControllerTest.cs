using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Xunit;

namespace ServiceBricks.Storage.EntityFrameworkCore.Xunit
{
    [Collection(Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public class ExampleApiControllerTest : ApiControllerTest<ExampleDto>
    {
        public ExampleApiControllerTest()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(EntityFrameworkCoreStartup));
            TestManager = SystemManager.ServiceProvider.GetRequiredService<ITestManager<ExampleDto>>();
        }
    }
}