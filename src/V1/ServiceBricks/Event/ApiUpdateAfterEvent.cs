namespace ServiceBricks
{
    /// <summary>
    /// This event fires After Commitping a DTO to a domain object.
    /// </summary>
    /// <typeparam name="TDomain"></typeparam>
    public partial class ApiUpdateAfterEvent<TDomain, TDto> : DomainEvent<TDomain>
        where TDomain : IDomainObject<TDomain>
    {
        public ApiUpdateAfterEvent() : base()
        { }

        public ApiUpdateAfterEvent(TDomain domain, TDto dto) : base()
        {
            DomainObject = domain;
            DtoObject = dto;
        }

        public TDto DtoObject { get; set; }
    }
}