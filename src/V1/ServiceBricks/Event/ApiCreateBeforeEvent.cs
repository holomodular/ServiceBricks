namespace ServiceBricks
{
    /// <summary>
    /// This event fires Before creating a DTO to a domain object.
    /// </summary>
    /// <typeparam name="TDto"></typeparam>
    public partial class ApiCreateBeforeEvent<TDomain, TDto> : DomainEvent<TDomain>
        where TDto : IDataTransferObject
    {
        public ApiCreateBeforeEvent() : base()
        { }

        public ApiCreateBeforeEvent(TDomain domain, TDto dto) : base()
        {
            DomainObject = domain;
            DtoObject = dto;
        }

        public TDto DtoObject { get; set; }
    }
}