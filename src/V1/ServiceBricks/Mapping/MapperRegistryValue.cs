namespace ServiceBricks
{
    /// <summary>
    /// This is the value stored for a registry.
    /// </summary>
    public partial class MapperRegistryValue
    {
        /// <summary>
        /// The value
        /// </summary>
        public virtual Action<object, object> Mapper { get; set; }

        /// <summary>
        /// The value
        /// </summary>
        public virtual Type DestinationType { get; set; }
    }
}