namespace ServiceBricks
{
    /// <summary>
    /// This is a domain process.
    /// </summary>
    public interface IDomainProcess
    {
    }

    /// <summary>
    /// This is a domain process.
    /// </summary>
    /// <typeparam name="TDomainObject"></typeparam>
    public interface IDomainProcess<TDomainObject>
    {
        TDomainObject DomainObject { get; set; }
    }
}