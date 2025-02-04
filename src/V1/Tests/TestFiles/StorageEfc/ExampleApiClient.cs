﻿using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ServiceBricks.Xunit;

namespace ServiceBricks.Storage.EntityFrameworkCore.Xunit
{
    public class ExampleApiClient : ApiClient<ExampleDto>, IExampleApiClient
    {
        public ExampleApiClient(
            ILoggerFactory loggerFactory,
            IHttpClientFactory httpClientFactory,
            IOptions<ClientApiOptions> clientApiOptions)
            : base(loggerFactory, httpClientFactory, clientApiOptions.Value)
        {
            ApiResource = $"EntityFrameworkCore/Example";
        }
    }

    public class ExampleProcessQueueApiClient : ApiClient<ExampleWorkProcessDto>, IExampleProcessQueueApiClient
    {
        public ExampleProcessQueueApiClient(
            ILoggerFactory loggerFactory,
            IHttpClientFactory httpClientFactory,
            IOptions<ClientApiOptions> clientApiOptions)
            : base(loggerFactory, httpClientFactory, clientApiOptions.Value)
        {
            ApiResource = $"EntityFrameworkCore/ExampleProcessQueue";
        }
    }
}