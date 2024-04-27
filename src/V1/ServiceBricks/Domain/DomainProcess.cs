namespace ServiceBricks
{
    /// <summary>
    /// This is an event process raised in the platform.
    /// </summary>
    public partial class DomainProcess : IDomainProcess<object>, IDomainProcess
    {
        public virtual object DomainObject { get; set; }
    }

    /// <summary>
    /// This is an event process raised in the platform based on a domain object.
    /// </summary>
    /// <typeparam name="TDomainObject"></typeparam>
    public partial class DomainProcess<TDomainObject> : IDomainProcess<TDomainObject>, IDomainProcess
    {
        public virtual TDomainObject DomainObject { get; set; }
    }
}