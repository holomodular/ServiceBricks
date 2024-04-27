namespace ServiceBricks
{
    /// <summary>
    /// This event fires After creating and commiting a DTO to a domain object.
    /// </summary>
    /// <typeparam name="TDomain"></typeparam>
    public partial class ApiCreateAfterEvent<TDomain, TDto> : DomainEvent<TDomain>
        where TDomain : IDomainObject<TDomain>
    {
        public ApiCreateAfterEvent() : base()
        { }

        public ApiCreateAfterEvent(TDomain domain, TDto dto) : base()
        {
            DomainObject = domain;
            DtoObject = dto;
        }

        public TDto DtoObject { get; set; }
    }
}