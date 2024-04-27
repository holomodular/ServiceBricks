namespace ServiceBricks
{
    /// <summary>
    /// This is the base class that all domain lookup types should inherit from.
    /// </summary>
    /// <typeparam name="TDomainObject"></typeparam>
    public class DomainType : DomainObject<DomainType>, IDomainType
    {
        public int Key { get; set; }

        public string Name { get; set; }
    }
}