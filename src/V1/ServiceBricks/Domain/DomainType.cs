namespace ServiceBricks
{
    /// <summary>
    /// This is the base class that all domain lookup types should inherit from.
    /// </summary>
    /// <typeparam name="TDomainObject"></typeparam>
    public partial class DomainType : DomainObject<DomainType>, IDomainType
    {
        /// <summary>
        /// The key of the domain type.
        /// </summary>
        public int Key { get; set; }

        /// <summary>
        /// The name of the domain type.
        /// </summary>
        public string Name { get; set; }
    }
}