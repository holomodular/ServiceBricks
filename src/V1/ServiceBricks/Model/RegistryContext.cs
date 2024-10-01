namespace ServiceBricks.Business
{
    /// <summary>
    /// This is the value stored for a registry.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public partial class RegistryContext<TValue>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public RegistryContext()
        {
            DefinitionData = new Dictionary<string, object>();
        }

        /// <summary>
        /// The custom data.
        /// </summary>
        public virtual Dictionary<string, object> DefinitionData { get; set; }

        /// <summary>
        /// The value
        /// </summary>
        public virtual TValue Value { get; set; }
    }
}