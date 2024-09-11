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
        public virtual async Task GetStorageRepositorySuccess()
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

        [Fact]
        public virtual Task StorageGetConnectionStringSuccess()
        {
            IConfiguration configuration = SystemManager.ServiceProvider.GetRequiredService<IConfiguration>();
            string connectionString = configuration.GetCosmosConnectionString();
            Assert.True(!string.IsNullOrEmpty(connectionString));

            var database = configuration.GetCosmosDatabase();
            Assert.True(!string.IsNullOrEmpty(database));

            connectionString = configuration.GetGeneralConnectionString();
            Assert.True(!string.IsNullOrEmpty(connectionString));

            database = configuration.GetGeneralDatabase();
            Assert.True(!string.IsNullOrEmpty(database));

            connectionString = configuration.GetPostgresConnectionString();
            Assert.True(!string.IsNullOrEmpty(connectionString));

            connectionString = configuration.GetSqliteConnectionString();
            Assert.True(!string.IsNullOrEmpty(connectionString));

            connectionString = configuration.GetSqlServerConnectionString();
            Assert.True(!string.IsNullOrEmpty(connectionString));

            return Task.CompletedTask;
        }
    }
}