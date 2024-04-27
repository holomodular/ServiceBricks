namespace ServiceBricks
{
    /// <summary>
    /// This event fires BEFORE getting all domain objects with paging.
    /// </summary>
    /// <typeparam name="TDomainObject"></typeparam>
    public partial class DomainGetAllPagingBeforeEvent<TDomainObject> : DomainEvent<TDomainObject> where TDomainObject : IDomainObject<TDomainObject>
    {
        public DomainGetAllPagingBeforeEvent() : base()
        {
        }

        public DomainGetAllPagingBeforeEvent(int pageNumber, int pageSize) : base()
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}