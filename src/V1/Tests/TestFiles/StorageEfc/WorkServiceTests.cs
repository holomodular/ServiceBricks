using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ServiceBricks.Xunit;

namespace ServiceBricks.Storage.EntityFrameworkCore.Xunit
{
    [Collection(Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public partial class WorkServiceTests
    {
        public virtual ISystemManager SystemManager { get; set; }

        public WorkServiceTests()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(EntityFrameworkCoreStartup));
        }

        public class ExampleWorkService : WorkService<ExampleWorkProcessDto>
        {
            public static int ProcessCalled;
            private string _processname;

            public ExampleWorkService(ILoggerFactory loggerFactory, IApiService<ExampleWorkProcessDto> apiService, string processname) : base(loggerFactory, apiService)
            {
                _processname = processname;
            }

            /// <summary>
            /// Process the item.
            /// </summary>
            /// <param name="domainObject"></param>
            /// <returns></returns>
            public override async Task<IResponse> ProcessItemAsync(ExampleWorkProcessDto dto)
            {
                dto.Name = _processname;
                ProcessCalled++;
                await Task.Delay(100);
                return new Response();
            }
        }

        [Fact]
        public virtual async Task ExampleWorkServiceTests_ProcessOne()
        {
            var processApiService = SystemManager.ServiceProvider.GetRequiredService<IApiService<ExampleWorkProcessDto>>();
            var loggerFactory = SystemManager.ServiceProvider.GetRequiredService<ILoggerFactory>();

            // Create
            var respCreate = await processApiService.CreateAsync(new ExampleWorkProcessDto());

            // Assert
            Assert.True(ExampleWorkService.ProcessCalled == 0);

            var exampleWorker = new ExampleWorkService(loggerFactory, processApiService, Guid.NewGuid().ToString());
            await exampleWorker.ExecuteAsync(CancellationToken.None);

            // Assert
            Assert.True(ExampleWorkService.ProcessCalled == 1);

            // Cleanup
            ExampleWorkService.ProcessCalled = 0;
            await processApiService.DeleteAsync(respCreate.Item.StorageKey);
        }

        [Fact]
        public virtual async Task ExampleWorkServiceTests_ProcessTwo()
        {
            var processApiService = SystemManager.ServiceProvider.GetRequiredService<IApiService<ExampleWorkProcessDto>>();
            var loggerFactory = SystemManager.ServiceProvider.GetRequiredService<ILoggerFactory>();

            // Create
            var respCreateOne = await processApiService.CreateAsync(new ExampleWorkProcessDto());
            var respCreateTwo = await processApiService.CreateAsync(new ExampleWorkProcessDto());

            // Assert
            Assert.True(ExampleWorkService.ProcessCalled == 0);

            var exampleWorker = new ExampleWorkService(loggerFactory, processApiService, Guid.NewGuid().ToString());
            await exampleWorker.ExecuteAsync(CancellationToken.None);

            // Assert
            Assert.True(ExampleWorkService.ProcessCalled == 2);

            // Cleanup
            ExampleWorkService.ProcessCalled = 0;
            await processApiService.DeleteAsync(respCreateOne.Item.StorageKey);
            await processApiService.DeleteAsync(respCreateTwo.Item.StorageKey);
        }

        [Fact]
        public virtual async Task ExampleWorkServiceTests_FixOrphan()
        {
            var processApiService = SystemManager.ServiceProvider.GetRequiredService<IApiService<ExampleWorkProcessDto>>();
            var loggerFactory = SystemManager.ServiceProvider.GetRequiredService<ILoggerFactory>();

            // Create
            var respCreateOrphan = await processApiService.CreateAsync(new ExampleWorkProcessDto()
            {
                IsProcessing = true,
                ProcessDate = DateTimeOffset.UtcNow.Subtract(ExampleWorkService.ORPHANED_RECORD_TIMEOUT)
            });
            var respCreateTwo = await processApiService.CreateAsync(new ExampleWorkProcessDto());

            // Assert
            Assert.True(ExampleWorkService.ProcessCalled == 0);

            var exampleWorker = new ExampleWorkService(loggerFactory, processApiService, Guid.NewGuid().ToString());
            await exampleWorker.ExecuteAsync(CancellationToken.None);

            // Assert
            Assert.True(ExampleWorkService.ProcessCalled == 2);

            // Cleanup
            ExampleWorkService.ProcessCalled = 0;
            await processApiService.DeleteAsync(respCreateOrphan.Item.StorageKey);
            await processApiService.DeleteAsync(respCreateTwo.Item.StorageKey);
        }

        [Fact]
        public virtual async Task ExampleWorkServiceTests_DontFixOrphanDisabled()
        {
            var processApiService = SystemManager.ServiceProvider.GetRequiredService<IApiService<ExampleWorkProcessDto>>();
            var loggerFactory = SystemManager.ServiceProvider.GetRequiredService<ILoggerFactory>();

            // Create
            var respCreateOrphan = await processApiService.CreateAsync(new ExampleWorkProcessDto()
            {
                IsProcessing = true,
                ProcessDate = DateTimeOffset.UtcNow.Subtract(ExampleWorkService.ORPHANED_RECORD_TIMEOUT)
            });
            var respCreateTwo = await processApiService.CreateAsync(new ExampleWorkProcessDto());

            // Assert
            Assert.True(ExampleWorkService.ProcessCalled == 0);

            var exampleWorker = new ExampleWorkService(loggerFactory, processApiService, Guid.NewGuid().ToString());
            exampleWorker.FixOrphanedProcessingRecords = false;
            await exampleWorker.ExecuteAsync(CancellationToken.None);

            // Assert
            Assert.True(ExampleWorkService.ProcessCalled == 1);

            // Cleanup
            ExampleWorkService.ProcessCalled = 0;
            await processApiService.DeleteAsync(respCreateOrphan.Item.StorageKey);
            await processApiService.DeleteAsync(respCreateTwo.Item.StorageKey);
        }

        [Fact]
        public virtual async Task ExampleWorkServiceTests_DontFixOrphanNotDue()
        {
            var processApiService = SystemManager.ServiceProvider.GetRequiredService<IApiService<ExampleWorkProcessDto>>();
            var loggerFactory = SystemManager.ServiceProvider.GetRequiredService<ILoggerFactory>();

            // Create
            var respCreateOrphan = await processApiService.CreateAsync(new ExampleWorkProcessDto()
            {
                IsProcessing = true,
                ProcessDate = DateTimeOffset.UtcNow.Subtract(TimeSpan.FromSeconds(1))
            });
            var respCreateTwo = await processApiService.CreateAsync(new ExampleWorkProcessDto());

            // Assert
            Assert.True(ExampleWorkService.ProcessCalled == 0);

            var exampleWorker = new ExampleWorkService(loggerFactory, processApiService, Guid.NewGuid().ToString());
            await exampleWorker.ExecuteAsync(CancellationToken.None);

            // Assert
            Assert.True(ExampleWorkService.ProcessCalled == 1);

            // Cleanup
            ExampleWorkService.ProcessCalled = 0;
            await processApiService.DeleteAsync(respCreateOrphan.Item.StorageKey);
            await processApiService.DeleteAsync(respCreateTwo.Item.StorageKey);
        }

        [Fact]
        public virtual async Task ExampleWorkServiceTests_DontProcessFuture()
        {
            var processApiService = SystemManager.ServiceProvider.GetRequiredService<IApiService<ExampleWorkProcessDto>>();
            var loggerFactory = SystemManager.ServiceProvider.GetRequiredService<ILoggerFactory>();

            // Create
            var respFuture = await processApiService.CreateAsync(new ExampleWorkProcessDto()
            {
                IsProcessing = true,
                ProcessDate = DateTimeOffset.UtcNow.Subtract(TimeSpan.FromSeconds(1)),
                FutureProcessDate = DateTimeOffset.UtcNow.AddMinutes(1),
            });

            // Assert
            Assert.True(ExampleWorkService.ProcessCalled == 0);

            var exampleWorker = new ExampleWorkService(loggerFactory, processApiService, Guid.NewGuid().ToString());
            await exampleWorker.ExecuteAsync(CancellationToken.None);

            // Assert
            Assert.True(ExampleWorkService.ProcessCalled == 0);

            // Cleanup
            ExampleWorkService.ProcessCalled = 0;
            await processApiService.DeleteAsync(respFuture.Item.StorageKey);
        }
    }
}