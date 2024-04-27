namespace ServiceBricks
{
    /// <summary>
    /// This event fires AFTER getting a domain object.
    /// </summary>
    /// <typeparam name="TDomainObject"></typeparam>
    public partial class DomainGetItemAfterEvent<TDomainObject> : DomainEvent<TDomainObject> where TDomainObject : IDomainObject<TDomainObject>
    {
        public DomainGetItemAfterEvent() : base()
        {
        }

        public DomainGetItemAfterEvent(TDomainObject obj) : base()
        {
            DomainObject = obj;
        }
    }
}