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
            var options = new OptionsWrapper<ApiConfig>(new ApiConfig() { ReturnResponseObject = false });
            return new ExampleApiClient(
                serviceProvider.GetRequiredService<ILoggerFactory>(),
                serviceProvider.GetRequiredService<IHttpClientFactory>(),
                options);
        }

        public override IApiClient<ExampleDto> GetClientReturnResponse(IServiceProvider serviceProvider)
        {
            var options = new OptionsWrapper<ApiConfig>(new ApiConfig() { ReturnResponseObject = true });
            return new ExampleApiClient(
                serviceProvider.GetRequiredService<ILoggerFactory>(),
                serviceProvider.GetRequiredService<IHttpClientFactory>(),
                options);
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
}