namespace ServiceBricks
{
    /// <summary>
    /// This API event fires after create.
    /// </summary>
    /// <typeparam name="TDomain"></typeparam>
    public partial class ApiCreateAfterEvent<TDomain, TDto> : DomainEvent<TDomain>
        where TDomain : IDomainObject<TDomain>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="dto"></param>
        public ApiCreateAfterEvent(TDomain domain, TDto dto) : base()
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