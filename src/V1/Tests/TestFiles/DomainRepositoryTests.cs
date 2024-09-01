using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ServiceQuery;

namespace ServiceBricks.Xunit
{
    [Collection(Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public partial class DomainRepositoryTests
    {
        public virtual ISystemManager SystemManager { get; set; }

        public DomainRepositoryTests()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(ServiceBricksStartup));
        }

        public class ExampleStorageRepository : IStorageRepository<ExampleDomain>
        {
            public bool CreateCalled = false;

            public IResponse Create(ExampleDomain model)
            {
                CreateCalled = true;
                return new Response();
            }

            public bool CreateAsyncCalled = false;

            public Task<IResponse> CreateAsync(ExampleDomain model)
            {
                CreateAsyncCalled = true;
                return Task.FromResult<IResponse>(new Response());
            }

            public bool DeleteCalled = false;

            public IResponse Delete(ExampleDomain model)
            {
                DeleteCalled = true;
                return new Response();
            }

            public bool DeleteAsyncCalled = false;

            public Task<IResponse> DeleteAsync(ExampleDomain model)
            {
                DeleteAsyncCalled = true;
                return Task.FromResult<IResponse>(new Response());
            }

            public bool GetCalled = false;

            public IResponseItem<ExampleDomain> Get(ExampleDomain model)
            {
                GetCalled = true;
                return new ResponseItem<ExampleDomain>(new ExampleDomain());
            }

            public bool GetAsyncCalled = false;

            public Task<IResponseItem<ExampleDomain>> GetAsync(ExampleDomain model)
            {
                GetAsyncCalled = true;
                return Task.FromResult<IResponseItem<ExampleDomain>>(new ResponseItem<ExampleDomain>(new ExampleDomain()));
            }

            public IStorageRepository<ExampleDomain> GetStorageRepository()
            {
                return this;
            }

            public bool QueryCalled = false;

            public IResponseItem<ServiceQueryResponse<ExampleDomain>> Query(ServiceQueryRequest request)
            {
                QueryCalled = true;
                return new ResponseItem<ServiceQueryResponse<ExampleDomain>>(new ServiceQueryResponse<ExampleDomain>());
            }

            public bool QueryAsyncCalled = false;

            public Task<IResponseItem<ServiceQueryResponse<ExampleDomain>>> QueryAsync(ServiceQueryRequest request)
            {
                QueryAsyncCalled = true;
                return Task.FromResult<IResponseItem<ServiceQueryResponse<ExampleDomain>>>(new ResponseItem<ServiceQueryResponse<ExampleDomain>>(new ServiceQueryResponse<ExampleDomain>()));
            }

            public bool UpdateCalled = false;

            public IResponse Update(ExampleDomain model)
            {
                UpdateCalled = true;
                return new Response();
            }

            public bool UpdateAsyncCalled = false;

            public Task<IResponse> UpdateAsync(ExampleDomain model)
            {
                UpdateAsyncCalled = true;
                return Task.FromResult<IResponse>(new Response());
            }
        }

        [Fact]
        public virtual Task DomainCallStorageSuccess()
        {
            var registry = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleRegistry>();
            var businessRuleService = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleService>();

            var storageRepository = new ExampleStorageRepository();
            var domainRepo = new DomainRepository<ExampleDomain>(
                SystemManager.ServiceProvider.GetRequiredService<ILoggerFactory>(),
                businessRuleService,
                storageRepository);
            var domain = new ExampleDomain();

            // Create
            Assert.True(!storageRepository.CreateCalled);
            domainRepo.Create(domain);
            Assert.True(storageRepository.CreateCalled);

            // Update
            Assert.True(!storageRepository.UpdateCalled);
            domainRepo.Update(domain);
            Assert.True(storageRepository.UpdateCalled);

            // Delete
            Assert.True(!storageRepository.DeleteCalled);
            domainRepo.Delete(domain);
            Assert.True(storageRepository.DeleteCalled);

            // Get
            Assert.True(!storageRepository.GetCalled);
            domainRepo.Get(domain);
            Assert.True(storageRepository.GetCalled);

            // Query
            Assert.True(!storageRepository.QueryCalled);
            domainRepo.Query(new ServiceQueryRequest());
            Assert.True(storageRepository.QueryCalled);

            return Task.CompletedTask;
        }

        [Fact]
        public virtual async Task DomainCallStorageSuccessAsync()
        {
            var registry = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleRegistry>();
            var businessRuleService = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleService>();

            var storageRepository = new ExampleStorageRepository();
            var domainRepo = new DomainRepository<ExampleDomain>(
                SystemManager.ServiceProvider.GetRequiredService<ILoggerFactory>(),
                businessRuleService,
                storageRepository);
            var domain = new ExampleDomain();

            // Create
            Assert.True(!storageRepository.CreateAsyncCalled);
            await domainRepo.CreateAsync(domain);
            Assert.True(storageRepository.CreateAsyncCalled);

            // Update
            Assert.True(!storageRepository.UpdateAsyncCalled);
            await domainRepo.UpdateAsync(domain);
            Assert.True(storageRepository.UpdateAsyncCalled);

            // Delete
            Assert.True(!storageRepository.DeleteAsyncCalled);
            await domainRepo.DeleteAsync(domain);
            Assert.True(storageRepository.DeleteAsyncCalled);

            // Get
            Assert.True(!storageRepository.GetAsyncCalled);
            await domainRepo.GetAsync(domain);
            Assert.True(storageRepository.GetAsyncCalled);

            // Query
            Assert.True(!storageRepository.QueryAsyncCalled);
            await domainRepo.QueryAsync(new ServiceQueryRequest());
            Assert.True(storageRepository.QueryAsyncCalled);
        }

        [Fact]
        public virtual Task DomainCallEventsSuccess()
        {
            var registry = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleRegistry>();
            var businessRuleService = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleService>();

            var storageRepository = new ExampleStorageRepository();
            var domainRepo = new DomainRepository<ExampleDomain>(
                SystemManager.ServiceProvider.GetRequiredService<ILoggerFactory>(),
                businessRuleService,
                storageRepository);
            var domain = new ExampleDomain();

            // Create
            Assert.True(!storageRepository.CreateCalled);
            domainRepo.Create(domain);
            Assert.True(storageRepository.CreateCalled);

            // Update
            Assert.True(!storageRepository.UpdateCalled);
            domainRepo.Update(domain);
            Assert.True(storageRepository.UpdateCalled);

            // Delete
            Assert.True(!storageRepository.DeleteCalled);
            domainRepo.Delete(domain);
            Assert.True(storageRepository.DeleteCalled);

            // Get
            Assert.True(!storageRepository.GetCalled);
            domainRepo.Get(domain);
            Assert.True(storageRepository.GetCalled);

            // Query
            Assert.True(!storageRepository.QueryCalled);
            domainRepo.Query(new ServiceQueryRequest());
            Assert.True(storageRepository.QueryCalled);

            return Task.CompletedTask;
        }
    }
}