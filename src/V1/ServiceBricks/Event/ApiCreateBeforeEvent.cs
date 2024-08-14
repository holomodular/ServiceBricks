namespace ServiceBricks
{
    /// <summary>
    /// This API event fires before create.
    /// </summary>
    /// <typeparam name="TDto"></typeparam>
    public partial class ApiCreateBeforeEvent<TDomain, TDto> : DomainEvent<TDomain>
        where TDto : IDataTransferObject
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="dto"></param>
        public ApiCreateBeforeEvent(TDomain domain, TDto dto) : base()
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