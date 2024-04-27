namespace ServiceBricks
{
    /// <summary>
    /// This event fires AFTER getting a domain object.
    /// </summary>
    /// <typeparam name="TDto"></typeparam>
    public partial class ApiGetItemAfterEvent<TDto> : DomainEvent<TDto>
        where TDto : IDataTransferObject
    {
        public ApiGetItemAfterEvent() : base()
        {
        }

        public ApiGetItemAfterEvent(TDto obj) : base()
        {
            DomainObject = obj;
        }
    }
}