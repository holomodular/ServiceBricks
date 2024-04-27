namespace ServiceBricks
{
    /// <summary>
    /// This event fires BEFORE inserting a domain object.
    /// </summary>
    /// <typeparam name="TDomainObject"></typeparam>
    public partial class DomainCreateBeforeEvent<TDomainObject> : DomainEvent<TDomainObject> where TDomainObject : IDomainObject<TDomainObject>
    {
        public DomainCreateBeforeEvent() : base()
        { }

        public DomainCreateBeforeEvent(TDomainObject obj) : base()
        {
            DomainObject = obj;
        }
    }
}