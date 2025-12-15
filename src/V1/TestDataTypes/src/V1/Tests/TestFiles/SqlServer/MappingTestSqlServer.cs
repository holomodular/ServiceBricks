using Microsoft.Extensions.DependencyInjection;

namespace ServiceBricks.Xunit.Integration
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public class MappingTestSqlServer
    {
        public virtual ISystemManager SystemManager { get; set; }

        public MappingTestSqlServer()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(StartupSqlServer));
        }

        [Fact]
        public virtual Task ValidateMapperConfiguration()
        {
            var mapper = SystemManager.ServiceProvider.GetRequiredService<IMapper>();

            return Task.CompletedTask;
        }
    }
}
