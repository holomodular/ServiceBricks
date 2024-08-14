using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;

namespace ServiceBricks.ServiceBus.Azure
{
    /// <summary>
    /// The ServiceBus connection interface.
    /// </summary>
    public partial interface IServiceBusConnection : IAsyncDisposable
    {
        /// <summary>
        /// The ServiceBus client.
        /// </summary>
        ServiceBusClient Client { get; }

        /// <summary>
        /// The administration client.
        /// </summary>
        ServiceBusAdministrationClient AdministrationClient { get; }
    }
}