namespace ServiceBricks
{
    /// <summary>
    /// This API event fires after get
    /// </summary>
    /// <typeparam name="TDto"></typeparam>
    public partial class ApiGetAfterEvent<TDto> : DomainEvent<TDto>
        where TDto : IDataTransferObject
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="obj"></param>
        public ApiGetAfterEvent(TDto obj) : base()
        {
            DomainObject = obj;
        }
    }
}