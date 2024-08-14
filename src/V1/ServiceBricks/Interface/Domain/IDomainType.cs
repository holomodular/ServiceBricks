namespace ServiceBricks
{
    /// <summary>
    /// This is a domain lookup type.
    /// </summary>
    public partial interface IDomainType
    {
        /// <summary>
        /// The key
        /// </summary>
        public int Key { get; set; }

        /// <summary>
        /// The name
        /// </summary>
        public string Name { get; set; }
    }
}