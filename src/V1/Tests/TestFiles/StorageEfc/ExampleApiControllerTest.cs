using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Xunit;

namespace ServiceBricks.Storage.EntityFrameworkCore.Xunit
{
    public class ExampleApiControllerTest : ApiControllerTest<ExampleDto>
    {
        public ExampleApiControllerTest()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(EntityFrameworkCoreStartup));
            TestManager = SystemManager.ServiceProvider.GetRequiredService<ITestManager<ExampleDto>>();
        }
    }
}