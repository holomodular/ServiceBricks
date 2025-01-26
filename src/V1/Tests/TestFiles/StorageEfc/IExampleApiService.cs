using ServiceBricks.Xunit;

namespace ServiceBricks.Storage.EntityFrameworkCore.Xunit
{
    public interface IExampleApiService : IApiService<ExampleDto>
    {
    }

    public interface IExampleProcessQueueApiService : IApiService<ExampleWorkProcessDto>
    {
    }
}