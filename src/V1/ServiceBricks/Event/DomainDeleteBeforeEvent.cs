namespace ServiceBricks
{
    /// <summary>
    /// This event fires BEFORE a domain object is deleted.
    /// </summary>
    /// <typeparam name="TDomainObject"></typeparam>
    public partial class DomainDeleteBeforeEvent<TDomainObject> : DomainEvent<TDomainObject> where TDomainObject : IDomainObject<TDomainObject>
    {
        public DomainDeleteBeforeEvent() : base()
        {
        }

        public DomainDeleteBeforeEvent(TDomainObject obj) : base()
        {
            DomainObject = obj;
        }
    }
}