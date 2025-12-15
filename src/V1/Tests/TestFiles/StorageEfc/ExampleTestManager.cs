using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ServiceBricks.Xunit;
using ServiceQuery;

namespace ServiceBricks.Storage.EntityFrameworkCore.Xunit
{
    public class ExampleTestManager : TestManager<ExampleDto>
    {
        public override ExampleDto GetMaximumDataObject()
        {
            var model = new ExampleDto()
            {
                Name = Guid.NewGuid().ToString(),
            };
            return model;
        }

        public override IApiController<ExampleDto> GetController(IServiceProvider serviceProvider)
        {
            var options = new OptionsWrapper<ApiOptions>(new ApiOptions() { ReturnResponseObject = false });
            return new ExampleApiController(serviceProvider.GetRequiredService<IExampleApiService>(), options);
        }

        public override IApiController<ExampleDto> GetControllerReturnResponse(IServiceProvider serviceProvider)
        {
            var options = new OptionsWrapper<ApiOptions>(new ApiOptions() { ReturnResponseObject = true });
            return new ExampleApiController(serviceProvider.GetRequiredService<IExampleApiService>(), options);
        }

        public override IApiClient<ExampleDto> GetClient(IServiceProvider serviceProvider)
        {
            var appConfig = serviceProvider.GetRequiredService<IConfiguration>();
            var config = new ConfigurationBuilder()
                .AddConfiguration(appConfig)
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    { ServiceBricksConstants.APPSETTING_CLIENT_APIOPTIONS + ":ReturnResponseObject", "false" },
                })
                .Build();

            return new ExampleApiClient(
                serviceProvider.GetRequiredService<ILoggerFactory>(),
                serviceProvider.GetRequiredService<IHttpClientFactory>(),
                config);
        }

        public override IApiClient<ExampleDto> GetClientReturnResponse(IServiceProvider serviceProvider)
        {
            var appConfig = serviceProvider.GetRequiredService<IConfiguration>();
            var config = new ConfigurationBuilder()
                .AddConfiguration(appConfig)
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    { ServiceBricksConstants.APPSETTING_CLIENT_APIOPTIONS + ":ReturnResponseObject", "true" },
                })
                .Build();

            return new ExampleApiClient(
                serviceProvider.GetRequiredService<ILoggerFactory>(),
                serviceProvider.GetRequiredService<IHttpClientFactory>(),
                config);
        }

        public override IApiService<ExampleDto> GetService(IServiceProvider serviceProvider)
        {
            return serviceProvider.GetRequiredService<IExampleApiService>();
        }

        public override void UpdateObject(ExampleDto dto)
        {
            dto.Name = Guid.NewGuid().ToString();
        }

        public override void ValidateObjects(ExampleDto clientDto, ExampleDto serviceDto, HttpMethod method)
        {
            //PrimaryKeyRule
            if (method == HttpMethod.Post)
                Assert.True(!string.IsNullOrEmpty(serviceDto.StorageKey));
            else
                Assert.True(serviceDto.StorageKey == clientDto.StorageKey);

            Assert.True(serviceDto.Name == clientDto.Name);
        }

        public override List<ServiceQueryRequest> GetQueriesForObject(ExampleDto dto)
        {
            List<ServiceQueryRequest> queries = new List<ServiceQueryRequest>();

            var qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(ExampleDto.Name), dto.Name);
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(ExampleDto.StorageKey), dto.StorageKey);
            queries.Add(qb.Build());

            return queries;
        }
    }

    public class ExampleProcessQueueTestManager : TestManager<ExampleWorkProcessDto>
    {
        public override ExampleWorkProcessDto GetMaximumDataObject()
        {
            var model = new ExampleWorkProcessDto()
            {
                Name = Guid.NewGuid().ToString(),
            };
            return model;
        }

        public override IApiController<ExampleWorkProcessDto> GetController(IServiceProvider serviceProvider)
        {
            var options = new OptionsWrapper<ApiOptions>(new ApiOptions() { ReturnResponseObject = false });
            return new ExampleProcessQueueApiController(serviceProvider.GetRequiredService<IExampleProcessQueueApiService>(), options);
        }

        public override IApiController<ExampleWorkProcessDto> GetControllerReturnResponse(IServiceProvider serviceProvider)
        {
            var options = new OptionsWrapper<ApiOptions>(new ApiOptions() { ReturnResponseObject = true });
            return new ExampleProcessQueueApiController(serviceProvider.GetRequiredService<IExampleProcessQueueApiService>(), options);
        }

        public override IApiClient<ExampleWorkProcessDto> GetClient(IServiceProvider serviceProvider)
        {
            var appConfig = serviceProvider.GetRequiredService<IConfiguration>();
            var config = new ConfigurationBuilder()
                .AddConfiguration(appConfig)
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    { ServiceBricksConstants.APPSETTING_CLIENT_APIOPTIONS + ":ReturnResponseObject", "false" },
                })
                .Build();
            
            return new ExampleProcessQueueApiClient(
                serviceProvider.GetRequiredService<ILoggerFactory>(),
                serviceProvider.GetRequiredService<IHttpClientFactory>(),
                config);
        }

        public override IApiClient<ExampleWorkProcessDto> GetClientReturnResponse(IServiceProvider serviceProvider)
        {
            var appConfig = serviceProvider.GetRequiredService<IConfiguration>();
            var config = new ConfigurationBuilder()
                .AddConfiguration(appConfig)
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    { ServiceBricksConstants.APPSETTING_CLIENT_APIOPTIONS + ":ReturnResponseObject", "true" },
                })
                .Build();
            return new ExampleProcessQueueApiClient(
                serviceProvider.GetRequiredService<ILoggerFactory>(),
                serviceProvider.GetRequiredService<IHttpClientFactory>(),
                config);
        }

        public override IApiService<ExampleWorkProcessDto> GetService(IServiceProvider serviceProvider)
        {
            return serviceProvider.GetRequiredService<IExampleProcessQueueApiService>();
        }

        public override void UpdateObject(ExampleWorkProcessDto dto)
        {
            dto.Name = Guid.NewGuid().ToString();
        }

        public override void ValidateObjects(ExampleWorkProcessDto clientDto, ExampleWorkProcessDto serviceDto, HttpMethod method)
        {
            //PrimaryKeyRule
            if (method == HttpMethod.Post)
                Assert.True(!string.IsNullOrEmpty(serviceDto.StorageKey));
            else
                Assert.True(serviceDto.StorageKey == clientDto.StorageKey);

            Assert.True(serviceDto.Name == clientDto.Name);
        }

        public override List<ServiceQueryRequest> GetQueriesForObject(ExampleWorkProcessDto dto)
        {
            List<ServiceQueryRequest> queries = new List<ServiceQueryRequest>();

            var qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(ExampleWorkProcessDto.Name), dto.Name);
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(ExampleWorkProcessDto.StorageKey), dto.StorageKey);
            queries.Add(qb.Build());

            return queries;
        }
    }
}