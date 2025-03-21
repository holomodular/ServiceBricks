﻿using ServiceBricks.Xunit;

namespace ServiceBricks.Storage.EntityFrameworkCore.Xunit
{
    public interface IExampleApiClient : IApiClient<ExampleDto>
    {
    }

    public interface IExampleProcessQueueApiClient : IApiClient<ExampleWorkProcessDto>
    {
    }
}