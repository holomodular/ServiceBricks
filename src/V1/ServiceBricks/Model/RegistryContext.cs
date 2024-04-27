namespace ServiceBricks
{
    /// <summary>
    /// This is the value stored for a registry.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public class RegistryContext<TValue>
    {
        public RegistryContext()
        {
            Custom = new Dictionary<string, object>();
        }

        public Dictionary<string, object> Custom { get; set; }
        public TValue Value { get; set; }
    }
}