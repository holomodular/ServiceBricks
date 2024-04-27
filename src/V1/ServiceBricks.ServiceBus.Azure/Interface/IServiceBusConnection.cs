using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;

namespace ServiceBricks.ServiceBus.Azure
{
    public interface IServiceBusConnection : IAsyncDisposable
    {
        ServiceBusClient Client { get; }
        ServiceBusAdministrationClient AdministrationClient { get; }
    }
}