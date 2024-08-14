namespace ServiceBricks
{
    /// <summary>
    /// This is a domain process.
    /// </summary>
    public partial interface IDomainProcess
    {
    }

    /// <summary>
    /// This is a domain process.
    /// </summary>
    /// <typeparam name="TDomainObject"></typeparam>
    public partial interface IDomainProcess<TDomainObject>
    {
        /// <summary>
        /// The domain object.
        /// </summary>
        TDomainObject DomainObject { get; set; }
    }
}