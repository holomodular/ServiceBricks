namespace ServiceBricks
{
    /// <summary>
    /// This API event fires after get
    /// </summary>
    /// <typeparam name="TDto"></typeparam>
    public partial class ApiGetAfterEvent<TDomain, TDto> : DomainEvent<TDomain>
        where TDto : IDataTransferObject
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="obj"></param>
        public ApiGetAfterEvent(TDomain domain, TDto dto) : base()
        {
            DomainObject = domain;
            Dto = dto;
        }

        /// <summary>
        /// The data transfer object.
        /// </summary>
        public virtual TDto Dto { get; set; }
    }
}