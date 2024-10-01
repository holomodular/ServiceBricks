using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ServiceQuery;

namespace ServiceBricks.Xunit
{
    [Collection(Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public partial class DomainQueueProcessTests
    {
        public virtual ISystemManager SystemManager { get; set; }

        public DomainQueueProcessTests()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(ServiceBricksStartup));
        }

        public class ExampleProcessQueueApiService : ApiService<ExampleProcessQueueDomain, ExampleDto>
        {
            public ExampleProcessQueueApiService(
                IMapper mapper,
                IBusinessRuleService businessRuleService,
                IDomainRepository<ExampleProcessQueueDomain> repository)
                : base(mapper, businessRuleService, repository)
            {
            }
        }

        public class ExampleProcessQueueStorageRepository : IStorageRepository<ExampleProcessQueueDomain>, IDomainProcessQueueStorageRepository<ExampleProcessQueueDomain>
        {
            public bool CreateCalled = false;

            public IResponse Create(ExampleProcessQueueDomain model)
            {
                CreateCalled = true;
                return new Response();
            }

            public bool CreateAsyncCalled = false;

            public Task<IResponse> CreateAsync(ExampleProcessQueueDomain model)
            {
                CreateAsyncCalled = true;
                return Task.FromResult<IResponse>(new Response());
            }

            public bool DeleteCalled = false;

            public IResponse Delete(ExampleProcessQueueDomain model)
            {
                DeleteCalled = true;
                return new Response();
            }

            public bool DeleteAsyncCalled = false;

            public Task<IResponse> DeleteAsync(ExampleProcessQueueDomain model)
            {
                DeleteAsyncCalled = true;
                return Task.FromResult<IResponse>(new Response());
            }

            public bool GetCalled = false;

            public IResponseItem<ExampleProcessQueueDomain> Get(ExampleProcessQueueDomain model)
            {
                GetCalled = true;
                return new ResponseItem<ExampleProcessQueueDomain>(new ExampleProcessQueueDomain() { Key = 1 });
            }

            public bool GetAsyncCalled = false;

            public Task<IResponseItem<ExampleProcessQueueDomain>> GetAsync(ExampleProcessQueueDomain model)
            {
                GetAsyncCalled = true;
                return Task.FromResult<IResponseItem<ExampleProcessQueueDomain>>(new ResponseItem<ExampleProcessQueueDomain>(new ExampleProcessQueueDomain() { Key = 1 }));
            }

            public IStorageRepository<ExampleProcessQueueDomain> GetStorageRepository()
            {
                return this;
            }

            public bool QueryCalled = false;

            public IResponseItem<ServiceQueryResponse<ExampleProcessQueueDomain>> Query(ServiceQueryRequest request)
            {
                QueryCalled = true;
                return new ResponseItem<ServiceQueryResponse<ExampleProcessQueueDomain>>(new ServiceQueryResponse<ExampleProcessQueueDomain>());
            }

            public bool QueryAsyncCalled = false;

            public Task<IResponseItem<ServiceQueryResponse<ExampleProcessQueueDomain>>> QueryAsync(ServiceQueryRequest request)
            {
                QueryAsyncCalled = true;
                return Task.FromResult<IResponseItem<ServiceQueryResponse<ExampleProcessQueueDomain>>>(new ResponseItem<ServiceQueryResponse<ExampleProcessQueueDomain>>(new ServiceQueryResponse<ExampleProcessQueueDomain>()));
            }

            public bool UpdateCalled = false;

            public IResponse Update(ExampleProcessQueueDomain model)
            {
                UpdateCalled = true;
                return new Response();
            }

            public bool UpdateAsyncCalled = false;

            public Task<IResponse> UpdateAsync(ExampleProcessQueueDomain model)
            {
                UpdateAsyncCalled = true;
                return Task.FromResult<IResponse>(new Response());
            }

            public bool GetQueueItemsCalled = false;
            public bool CallMeOnceError = false;
            public bool CallMeOnceSuccess = false;

            public Task<IResponseList<ExampleProcessQueueDomain>> GetQueueItemsAsync(int batchNumberToTake, bool pickupErrors, DateTimeOffset errorPickupCutoffDate)
            {
                GetQueueItemsCalled = true;
                ResponseList<ExampleProcessQueueDomain> response = new ResponseList<ExampleProcessQueueDomain>();

                if (pickupErrors)
                {
                    if (!CallMeOnceError)
                    {
                        response.List.Add(
                            new ExampleProcessQueueDomain()
                            {
                                IsError = true,
                                ProcessDate = DateTimeOffset.UtcNow.Subtract(TimeSpan.FromHours(1))
                            });
                        response.List.Add(
                            new ExampleProcessQueueDomain()
                            {
                                IsError = true,
                                ProcessDate = DateTimeOffset.UtcNow.Subtract(TimeSpan.FromHours(1)),
                                RetryCount = DomainProcessQueueService<ExampleProcessQueueDomain>.RETRY_NUMBER - 2
                            });
                        response.List.Add(
                            new ExampleProcessQueueDomain()
                            {
                                IsError = true,
                                ProcessDate = DateTimeOffset.UtcNow.Subtract(TimeSpan.FromHours(1)),
                                RetryCount = DomainProcessQueueService<ExampleProcessQueueDomain>.RETRY_NUMBER - 1
                            });
                        response.List.Add(
                            new ExampleProcessQueueDomain()
                            {
                                IsError = true,
                                ProcessDate = DateTimeOffset.UtcNow.Subtract(TimeSpan.FromHours(1)),
                                RetryCount = DomainProcessQueueService<ExampleProcessQueueDomain>.RETRY_NUMBER
                            });
                        response.List.Add(
                            new ExampleProcessQueueDomain()
                            {
                                IsError = true,
                                ProcessDate = DateTimeOffset.UtcNow.Subtract(TimeSpan.FromHours(1)),
                                RetryCount = DomainProcessQueueService<ExampleProcessQueueDomain>.RETRY_NUMBER + 1
                            });
                        CallMeOnceError = true;
                    }
                }
                else
                {
                    if (!CallMeOnceSuccess)
                    {
                        response.List.Add(new ExampleProcessQueueDomain());
                        CallMeOnceSuccess = true;
                    }
                }

                return Task.FromResult<IResponseList<ExampleProcessQueueDomain>>(response);
            }
        }

        public class ExampleProcessQueueService : DomainProcessQueueService<ExampleProcessQueueDomain>
        {
            public ExampleProcessQueueService(
                ILoggerFactory loggerFactory,
                IDomainProcessQueueStorageRepository<ExampleProcessQueueDomain> repository)
                : base(loggerFactory, repository)
            {
            }

            public bool ProcessItemAsyncCalled = false;

            public override Task<IResponse> ProcessItemAsync(ExampleProcessQueueDomain domainObject)
            {
                ProcessItemAsyncCalled = true;
                return Task.FromResult<IResponse>(new Response());
            }
        }

        [Fact]
        public virtual async Task DomainProcessQueueSuccess()
        {
            var businessRuleService = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleService>();
            var loggerFactory = SystemManager.ServiceProvider.GetRequiredService<ILoggerFactory>();

            var storageRepository = new ExampleProcessQueueStorageRepository();
            var domainRepo = new DomainRepository<ExampleProcessQueueDomain>(
                loggerFactory,
                businessRuleService,
                storageRepository);

            var exampleProcessQueueService = new ExampleProcessQueueService(
                loggerFactory,
                storageRepository);

            // Execute process
            CancellationTokenSource source = new CancellationTokenSource();
            await exampleProcessQueueService.ExecuteAsync(source.Token);

            // Assert
            Assert.True(exampleProcessQueueService.ProcessItemAsyncCalled);
            Assert.True(storageRepository.GetQueueItemsCalled);
            Assert.True(storageRepository.CallMeOnceError);
            Assert.True(storageRepository.CallMeOnceSuccess);
        }

        [Fact]
        public virtual Task GetQueueItemsSuccess()
        {
            var query = DomainProcessQueueService<ExampleProcessQueueDomain>.GetQueueItemsQuery(1, false, DateTimeOffset.Now);

            // Assert
            Assert.True(query != null);
            Assert.True(query.Filters != null);
            Assert.True(query.Filters.Count > 0);

            return Task.CompletedTask;
        }
    }
}