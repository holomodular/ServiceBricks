namespace ServiceBricks
{
    /// <summary>
    /// This event fires After creating and commiting a DTO to a domain object.
    /// </summary>
    /// <typeparam name="TDomain"></typeparam>
    public partial class ApiDeleteBeforeEvent<TDomain, TDto> : DomainEvent<TDomain>
        where TDomain : IDomainObject<TDomain>
    {
        public ApiDeleteBeforeEvent() : base()
        { }

        public ApiDeleteBeforeEvent(TDomain domain, TDto dto) : base()
        {
            DomainObject = domain;
            DtoObject = dto;
        }

        public TDto DtoObject { get; set; }
    }
}