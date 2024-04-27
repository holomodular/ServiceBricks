namespace ServiceBricks
{
    /// <summary>
    /// This event fires AFTER inserting a domain object.
    /// </summary>
    /// <typeparam name="TDomainObject"></typeparam>
    public partial class DomainCreateAfterEvent<TDomainObject> : DomainEvent<TDomainObject> where TDomainObject : IDomainObject<TDomainObject>
    {
        public DomainCreateAfterEvent() : base()
        { }

        public DomainCreateAfterEvent(TDomainObject obj) : base()
        {
            DomainObject = obj;
        }
    }
}