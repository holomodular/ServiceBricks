namespace ServiceBricks
{
    /// <summary>
    /// This event fires AFTER getting all domain objects.
    /// </summary>
    /// <typeparam name="TDomainObject"></typeparam>
    public partial class ApiGetAllAfterEvent<TDto> : DomainEvent<TDto>
        where TDto : IDataTransferObject
    {
        public ApiGetAllAfterEvent() : base()
        {
        }

        public ApiGetAllAfterEvent(IResponseList<TDto> response) : base()
        {
            Response = response;
        }

        public virtual IResponseList<TDto> Response { get; set; }
    }
}