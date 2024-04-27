namespace ServiceBricks
{
    /// <summary>
    /// This event fires AFTER getting all domain objects with paging.
    /// </summary>
    /// <typeparam name="TDto"></typeparam>
    public partial class ApiGetAllPagingAfterEvent<TDto> : DomainEvent<TDto>
        where TDto : IDataTransferObject
    {
        public ApiGetAllPagingAfterEvent() : base()
        {
        }

        public ApiGetAllPagingAfterEvent(IResponseAggregateCountList<TDto> response) : base()
        {
            Response = response;
        }

        public virtual IResponseAggregateCountList<TDto> Response { get; set; }
    }
}