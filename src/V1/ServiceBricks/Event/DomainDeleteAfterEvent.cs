namespace ServiceBricks
{
    /// <summary>
    /// This event fires AFTER a domain object is deleted.
    /// </summary>
    /// <typeparam name="TDomainObject"></typeparam>
    public partial class DomainDeleteAfterEvent<TDomainObject> : DomainEvent<TDomainObject> where TDomainObject : IDomainObject<TDomainObject>
    {
        public DomainDeleteAfterEvent() : base()
        {
        }

        public DomainDeleteAfterEvent(TDomainObject obj) : base()
        {
            DomainObject = obj;
        }

        public virtual IResponse Response { get; set; }
    }
}