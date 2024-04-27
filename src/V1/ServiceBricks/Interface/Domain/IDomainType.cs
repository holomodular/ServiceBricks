namespace ServiceBricks
{
    /// <summary>
    /// This is a domain lookup type.
    /// </summary>
    public interface IDomainType
    {
        public int Key { get; set; }
        public string Name { get; set; }
    }
}