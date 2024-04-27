namespace ServiceBricks
{
    /// <summary>
    /// This event fires After creating and commiting a DTO to a domain object.
    /// </summary>
    /// <typeparam name="TDomain"></typeparam>
    public partial class ApiDeleteAfterEvent<TDomain, TDto> : DomainEvent<TDomain>
        where TDomain : IDomainObject<TDomain>
    {
        public ApiDeleteAfterEvent() : base()
        { }

        public ApiDeleteAfterEvent(TDomain domain, TDto dto) : base()
        {
            DomainObject = domain;
            DtoObject = dto;
        }

        public TDto DtoObject { get; set; }
    }
}