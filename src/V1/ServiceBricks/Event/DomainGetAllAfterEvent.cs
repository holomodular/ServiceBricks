namespace ServiceBricks
{
    /// <summary>
    /// This event fires AFTER getting all domain objects.
    /// </summary>
    /// <typeparam name="TDomainObject"></typeparam>
    public partial class DomainGetAllAfterEvent<TDomainObject> : DomainEvent<TDomainObject> where TDomainObject : IDomainObject<TDomainObject>
    {
        public DomainGetAllAfterEvent() : base()
        {
        }

        public virtual IResponseList<TDomainObject> Response { get; set; }
    }
}