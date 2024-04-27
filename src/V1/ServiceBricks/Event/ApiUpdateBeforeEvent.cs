namespace ServiceBricks
{
    /// <summary>
    /// This event fires Before creating a DTO to a domain object.
    /// </summary>
    /// <typeparam name="TDto"></typeparam>
    public partial class ApiUpdateBeforeEvent<TDomain, TDto> : DomainEvent<TDomain>
        where TDto : IDataTransferObject
    {
        public ApiUpdateBeforeEvent() : base()
        { }

        public ApiUpdateBeforeEvent(TDomain domain, TDto dto) : base()
        {
            DomainObject = domain;
            DtoObject = dto;
        }

        public TDto DtoObject { get; set; }
    }
}