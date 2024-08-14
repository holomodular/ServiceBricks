namespace ServiceBricks.ServiceBus.Azure
{
    /// <summary>
    /// These are constants used by the ServiceBricks ServiceBus Azure module.
    /// </summary>
    public static partial class ServiceBusAzureConstants
    {
        /// <summary>
        /// Application setting for the connection string.
        /// </summary>
        public const string APPSETTINGS_CONNECTION_STRING = "ServiceBricks:ServiceBus:Azure:ConnectionString";

        /// <summary>
        /// Application setting for the queue name.
        /// </summary>
        public const string APPSETTINGS_QUEUE = "ServiceBricks:ServiceBus:Azure:Queue";

        /// <summary>
        /// Application setting for the topic name.
        /// </summary>
        public const string APPSETTINGS_TOPIC = "ServiceBricks:ServiceBus:Azure:Topic";

        /// <summary>
        /// Application setting for the subscription name.
        /// </summary>
        public const string APPSETTINGS_SUBSCRIPTION = "ServiceBricks:ServiceBus:Azure:Subscription";
    }
}