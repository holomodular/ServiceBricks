namespace ServiceBricks
{
    /// <summary>
    /// This event fires AFTER updating a domain object.
    /// </summary>
    /// <typeparam name="TDomainObject"></typeparam>
    public partial class DomainUpdateAfterEvent<TDomainObject> : DomainEvent<TDomainObject> where TDomainObject : IDomainObject<TDomainObject>
    {
        public DomainUpdateAfterEvent() : base()
        {
        }

        public DomainUpdateAfterEvent(TDomainObject obj) : base()
        {
            DomainObject = obj;
        }
    }
}