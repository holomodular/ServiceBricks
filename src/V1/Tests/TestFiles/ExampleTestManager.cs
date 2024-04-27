﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ServiceBricks.Xunit;
using ServiceQuery;

namespace ServiceBricks.Client.Xunit
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
            throw new NotImplementedException();
        }

        public override IApiController<ExampleDto> GetControllerReturnResponse(IServiceProvider serviceProvider)
        {
            throw new NotImplementedException();
        }

        public override IApiClient<ExampleDto> GetClient(IServiceProvider serviceProvider)
        {
            var apiconfig = serviceProvider.GetRequiredService<IOptions<ApiConfig>>().Value;
            apiconfig.ReturnResponseObject = false;
            var options = new OptionsWrapper<ApiConfig>(apiconfig);
            return new ExampleApiClient(
                serviceProvider.GetRequiredService<ILoggerFactory>(),
                serviceProvider.GetRequiredService<IHttpClientFactory>(),
                options.Value);
        }

        public override IApiClient<ExampleDto> GetClientReturnResponse(IServiceProvider serviceProvider)
        {
            var apiconfig = serviceProvider.GetRequiredService<IOptions<ApiConfig>>().Value;
            apiconfig.ReturnResponseObject = true;
            var options = new OptionsWrapper<ApiConfig>(apiconfig);
            return new ExampleApiClient(
                serviceProvider.GetRequiredService<ILoggerFactory>(),
                serviceProvider.GetRequiredService<IHttpClientFactory>(),
                options.Value);
        }

        public override IApiService<ExampleDto> GetService(IServiceProvider serviceProvider)
        {
            throw new NotImplementedException();
        }

        public override void UpdateObject(ExampleDto dto)
        {
            dto.Name = Guid.NewGuid().ToString();
        }

        public override void ValidateObjects(ExampleDto clientDto, ExampleDto serviceDto, HttpMethod method)
        {
            Assert.NotNull(clientDto);
            Assert.NotNull(serviceDto);

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