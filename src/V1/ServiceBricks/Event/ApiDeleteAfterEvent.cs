namespace ServiceBricks
{
    /// <summary>
    /// This API event fires after delete.
    /// </summary>
    /// <typeparam name="TDomain"></typeparam>
    public partial class ApiDeleteAfterEvent<TDomain, TDto> : DomainEvent<TDomain>
        where TDomain : IDomainObject<TDomain>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="dto"></param>
        public ApiDeleteAfterEvent(TDomain domain, TDto dto) : base()
        {
            DomainObject = domain;
            DtoObject = dto;
        }

        /// <summary>
        /// The data transfer object.
        /// </summary>
        public virtual TDto DtoObject { get; set; }
    }
}