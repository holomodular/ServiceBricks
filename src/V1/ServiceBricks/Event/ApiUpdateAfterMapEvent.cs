namespace ServiceBricks
{
    /// <summary>
    /// This API event fires after mapping during update.
    /// </summary>
    /// <typeparam name="TDto"></typeparam>
    public partial class ApiUpdateAfterMapEvent<TDomain, TDto> : DomainEvent<TDomain>
        where TDto : IDataTransferObject
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="dto"></param>
        public ApiUpdateAfterMapEvent(TDomain domain, TDto dto) : base()
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