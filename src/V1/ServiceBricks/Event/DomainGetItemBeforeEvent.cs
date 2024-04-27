namespace ServiceBricks
{
    /// <summary>
    /// This event fires BEFORE getting a domain object.
    /// </summary>
    /// <typeparam name="TDomainObject"></typeparam>
    public partial class DomainGetItemBeforeEvent<TDomainObject> : DomainEvent<TDomainObject> where TDomainObject : IDomainObject<TDomainObject>
    {
        public DomainGetItemBeforeEvent() : base()
        {
        }

        public DomainGetItemBeforeEvent(TDomainObject obj) : base()
        {
            DomainObject = obj;
        }
    }
}