using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using Microsoft.Extensions.Configuration;

namespace ServiceBricks.ServiceBus.Azure
{
    public class ServiceBusConnection : IServiceBusConnection
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private ServiceBusClient _client;
        private ServiceBusAdministrationClient _subscriptionClient;

        private bool _disposed;

        public ServiceBusConnection(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetValue<string>(ServiceBusAzureConstants.APPSETTINGS_CONNECTION_STRING);
            _subscriptionClient = new ServiceBusAdministrationClient(_connectionString);
            _client = new ServiceBusClient(_connectionString);
        }

        public ServiceBusClient Client
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

        public ServiceBusAdministrationClient AdministrationClient =>
            _subscriptionClient;

        public ServiceBusClient CreateModel()
        {
            if (_client.IsClosed)
            {
                _client = new ServiceBusClient(_connectionString);
            }

            return _client;
        }

        public async ValueTask DisposeAsync()
        {
            if (_disposed) return;

            _disposed = true;
            await _client.DisposeAsync();
        }
    }
}