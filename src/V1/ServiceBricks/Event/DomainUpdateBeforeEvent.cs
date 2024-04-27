namespace ServiceBricks
{
    /// <summary>
    /// This event fires BEFORE updating a domain object.
    /// </summary>
    /// <typeparam name="TDomainObject"></typeparam>
    public partial class DomainUpdateBeforeEvent<TDomainObject> : DomainEvent<TDomainObject> where TDomainObject : IDomainObject<TDomainObject>
    {
        public DomainUpdateBeforeEvent() : base()
        {
        }

        public DomainUpdateBeforeEvent(TDomainObject obj) : base()
        {
            DomainObject = obj;
        }
    }
}