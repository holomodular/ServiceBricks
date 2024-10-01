namespace ServiceBricks
{
    /// <summary>
    /// The module definition for the Service Bricks module.
    /// </summary>
    public partial class ServiceBusAzureModule : ServiceBricks.Module
    {
        /// <summary>
        /// Static instance
        /// </summary>
        public static ServiceBusAzureModule Instance = new ServiceBusAzureModule();

        /// <summary>
        /// Constructor.
        /// </summary>
        public ServiceBusAzureModule() : base()
        {
            StartPriority = BusinessRule.PRIORITY_AFTER_LOW;
        }
    }
}