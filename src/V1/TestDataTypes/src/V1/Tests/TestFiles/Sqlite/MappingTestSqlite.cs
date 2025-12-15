using Microsoft.Extensions.DependencyInjection;

namespace ServiceBricks.Xunit.Integration
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public class MappingTestSqlite
    {
        public virtual ISystemManager SystemManager { get; set; }

        public MappingTestSqlite()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(StartupSqlite));
        }

        [Fact]
        public virtual Task ValidateMapperConfiguration()
        {
            var mapper = SystemManager.ServiceProvider.GetRequiredService<IMapper>();

            return Task.CompletedTask;
        }
    }
}
