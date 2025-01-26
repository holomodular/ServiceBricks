using ServiceBricks.Xunit;

namespace ServiceBricks.Storage.EntityFrameworkCore.Xunit
{
    public interface IExampleApiController : IApiController<ExampleDto>
    {
    }

    public interface IExampleProcessQueueApiController : IApiController<ExampleWorkProcessDto>
    {
    }
}