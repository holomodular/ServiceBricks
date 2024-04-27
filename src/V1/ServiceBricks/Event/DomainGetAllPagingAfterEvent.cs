namespace ServiceBricks
{
    /// <summary>
    /// This event fires AFTER getting all domain objects with paging.
    /// </summary>
    /// <typeparam name="TDomainObject"></typeparam>
    public partial class DomainGetAllPagingAfterEvent<TDomainObject> : DomainEvent<TDomainObject> where TDomainObject : IDomainObject<TDomainObject>
    {
        public DomainGetAllPagingAfterEvent() : base()
        {
        }

        public virtual IResponseAggregateCountList<TDomainObject> Response { get; set; }
    }
}