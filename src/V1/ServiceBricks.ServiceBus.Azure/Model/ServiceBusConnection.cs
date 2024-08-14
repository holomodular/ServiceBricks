using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using Microsoft.Extensions.Configuration;

namespace ServiceBricks.ServiceBus.Azure
{
    /// <summary>
    /// The ServiceBusConnection class.
    /// </summary>
    public partial class ServiceBusConnection : IServiceBusConnection
    {
        protected readonly IConfiguration _configuration;
        protected readonly string _connectionString;

        protected ServiceBusClient _client;
        protected ServiceBusAdministrationClient _subscriptionClient;
        protected bool _disposed;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="configuration"></param>
        public ServiceBusConnection(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetValue<string>(ServiceBusAzureConstants.APPSETTINGS_CONNECTION_STRING);
            _subscriptionClient = new ServiceBusAdministrationClient(_connectionString);
            _client = new ServiceBusClient(_connectionString);
        }

        /// <summary>
        /// The client.
        /// </summary>
        public virtual ServiceBusClient Client
        {
            get
            {
                if (_client.IsClosed)
                {
                    _client = new ServiceBusClient(_connectionString);
                }
                return _client;
            }
        }

        /// <summary>
        /// The administration client.
        /// </summary>
        public virtual ServiceBusAdministrationClient AdministrationClient =>
            _subscriptionClient;

        /// <summary>
        /// Dispose the connection.
        /// </summary>
        /// <returns></returns>
        public virtual async ValueTask DisposeAsync()
        {
            if (_disposed) return;

            _disposed = true;
            await _client.DisposeAsync();
        }
    }
}