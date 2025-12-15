
using Microsoft.Extensions.DependencyInjection;

namespace ServiceBricks.Xunit.Integration
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public class MappingTestAzureDataTables
    {
        public virtual ISystemManager SystemManager { get; set; }

        public MappingTestAzureDataTables()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(StartupAzureDataTables));
        }

        [Fact]
        public virtual Task ValidateMapperConfiguration()
        {
            var mapper = SystemManager.ServiceProvider.GetRequiredService<IMapper>();

            return Task.CompletedTask;
        }
    }
}
