using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ServiceQuery;

namespace ServiceBricks.Xunit
{
    [Collection(Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public partial class ApiServiceTests
    {
        public virtual ISystemManager SystemManager { get; set; }

        public ApiServiceTests()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(ApiStartup));
        }

        public class ApiStartup : ServiceBricks.Startup
        {
            public ApiStartup(IConfiguration configuration) : base(configuration)
            {
            }

            public virtual void ConfigureDevelopmentServices(IServiceCollection services)
            {
                base.CustomConfigureServices(services);
                services.AddSingleton(Configuration);
                services.AddServiceBricks(Configuration);

                // API CONFIGS
                ModuleRegistry.Instance.Register(new TestModule());

                // Remove all background tasks/timers for unit testing

                // Register TestManagers

                services.AddServiceBricksComplete(Configuration);
            }

            public virtual void Configure(IApplicationBuilder app)
            {
                base.CustomConfigure(app);
                app.StartServiceBricks();
            }
        }

        public class ExampleApiService : ApiService<ExampleDomain, ExampleDto>
        {
            public ExampleApiService(
                IMapper mapper,
                IBusinessRuleService businessRuleService,
                IDomainRepository<ExampleDomain> repository)
                : base(mapper, businessRuleService, repository)
            {
            }
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
                return new ResponseItem<ExampleDomain>(new ExampleDomain() { Key = 1 });
            }

            public bool GetAsyncCalled = false;

            public Task<IResponseItem<ExampleDomain>> GetAsync(ExampleDomain model)
            {
                GetAsyncCalled = true;
                return Task.FromResult<IResponseItem<ExampleDomain>>(new ResponseItem<ExampleDomain>(new ExampleDomain() { Key = 1 }));
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
        public virtual Task ApiServiceCallStorageSuccess()
        {
            var businessRuleService = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleService>();

            var storageRepository = new ExampleStorageRepository();
            var domainRepo = new DomainRepository<ExampleDomain>(
                SystemManager.ServiceProvider.GetRequiredService<ILoggerFactory>(),
                businessRuleService,
                storageRepository);
            var apiservice = new ExampleApiService(
                SystemManager.ServiceProvider.GetRequiredService<IMapper>(),
                businessRuleService,
                domainRepo);
            var dto = new ExampleDto() { StorageKey = "1" };

            // Get
            Assert.True(!storageRepository.GetCalled);
            apiservice.Get(dto.StorageKey);
            Assert.True(storageRepository.GetCalled);
            storageRepository.GetCalled = false;

            // Create
            Assert.True(!storageRepository.CreateCalled);
            apiservice.Create(dto);
            Assert.True(storageRepository.CreateCalled);

            // Update
            Assert.True(!storageRepository.UpdateCalled);
            apiservice.Update(dto);
            Assert.True(storageRepository.UpdateCalled);
            Assert.True(storageRepository.GetCalled);
            storageRepository.GetCalled = false;

            // Delete
            Assert.True(!storageRepository.DeleteCalled);
            apiservice.Delete(dto.StorageKey);
            Assert.True(storageRepository.DeleteCalled);
            Assert.True(storageRepository.GetCalled);
            storageRepository.GetCalled = false;

            // Query
            Assert.True(!storageRepository.QueryCalled);
            apiservice.Query(new ServiceQueryRequest());
            Assert.True(storageRepository.QueryCalled);

            return Task.CompletedTask;
        }

        [Fact]
        public virtual async Task ApiServiceCallStorageSuccessAsync()
        {
            var businessRuleService = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleService>();

            var storageRepository = new ExampleStorageRepository();
            var domainRepo = new DomainRepository<ExampleDomain>(
                SystemManager.ServiceProvider.GetRequiredService<ILoggerFactory>(),
                businessRuleService,
                storageRepository);
            var apiservice = new ExampleApiService(
                SystemManager.ServiceProvider.GetRequiredService<IMapper>(),
                businessRuleService,
                domainRepo);
            var dto = new ExampleDto() { StorageKey = "1" };

            // Get
            Assert.True(!storageRepository.GetAsyncCalled);
            await apiservice.GetAsync(dto.StorageKey);
            Assert.True(storageRepository.GetAsyncCalled);
            storageRepository.GetAsyncCalled = false;

            // Create
            Assert.True(!storageRepository.CreateAsyncCalled);
            await apiservice.CreateAsync(dto);
            Assert.True(storageRepository.CreateAsyncCalled);

            // Update
            Assert.True(!storageRepository.UpdateAsyncCalled);
            await apiservice.UpdateAsync(dto);
            Assert.True(storageRepository.UpdateAsyncCalled);
            Assert.True(storageRepository.GetAsyncCalled);
            storageRepository.GetAsyncCalled = false;

            // Delete
            Assert.True(!storageRepository.DeleteAsyncCalled);
            await apiservice.DeleteAsync(dto.StorageKey);
            Assert.True(storageRepository.DeleteAsyncCalled);
            Assert.True(storageRepository.GetAsyncCalled);
            storageRepository.GetAsyncCalled = false;

            // Query
            Assert.True(!storageRepository.QueryAsyncCalled);
            await apiservice.QueryAsync(new ServiceQueryRequest());
            Assert.True(storageRepository.QueryAsyncCalled);
        }
    }
}