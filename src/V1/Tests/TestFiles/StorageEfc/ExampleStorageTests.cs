using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ServiceQuery;
using ServiceBricks.Xunit;

namespace ServiceBricks.Storage.EntityFrameworkCore.Xunit
{
    [Collection(Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public partial class ExampleStorageTests
    {
        public virtual ISystemManager SystemManager { get; set; }

        public ExampleStorageTests()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(EntityFrameworkCoreStartup));
        }

        [Fact]
        public virtual async Task ApplicationEmailSuccess()
        {
            var storage = SystemManager.ServiceProvider.GetRequiredService<IStorageRepository<ExampleDomain>>();

            var s = storage.GetStorageRepository();
            Assert.True(storage == s);

            var efcs = storage as EntityFrameworkCoreStorageRepository<ExampleDomain>;
            Assert.True(efcs != null);

            efcs.SaveChanges();

            await efcs.SaveChangesAsync();

            efcs.DetachAllEntities();
        }
    }
}