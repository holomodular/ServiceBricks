namespace ServiceBricks
{
    /// <summary>
    /// This event fires BEFORE getting all domain objects.
    /// </summary>
    /// <typeparam name="TDto"></typeparam>
    public partial class ApiGetAllBeforeEvent<TDto> : DomainEvent<TDto>
        where TDto : IDataTransferObject
    {
        public ApiGetAllBeforeEvent() : base()
        {
        }
    }
}