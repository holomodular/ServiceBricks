namespace ServiceBricks
{
    /// <summary>
    /// This event fires BEFORE getting all domain objects with paging.
    /// </summary>
    /// <typeparam name="TDto"></typeparam>
    public partial class ApiGetAllPagingBeforeEvent<TDto> : DomainEvent<TDto>
        where TDto : IDataTransferObject
    {
        public ApiGetAllPagingBeforeEvent() : base()
        { }

        public ApiGetAllPagingBeforeEvent(Paging paging) : base()
        {
            Paging = paging;
        }

        public Paging Paging { get; set; }
    }
}