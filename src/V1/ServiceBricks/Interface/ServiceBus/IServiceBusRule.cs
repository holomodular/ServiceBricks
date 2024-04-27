namespace ServiceBricks
{
    public interface IServiceBusRule
    {
    }

    public interface IServiceBusRule<TObj> : IServiceBusRule
    {
        Task HandleServiceBusMessage(TObj obj);
    }
}