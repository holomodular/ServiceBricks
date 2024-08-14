using ServiceQuery;

namespace ServiceBricks
{
    /// <summary>
    /// This API event fires before query.
    /// </summary>
    /// <typeparam name="TDto"></typeparam>
    public partial class ApiQueryBeforeEvent<TDto> : DomainEvent<TDto>
        where TDto : IDataTransferObject
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="request"></param>
        public ApiQueryBeforeEvent(ServiceQueryRequest request) : base()
        {
            ServiceQueryRequest = request;
        }

        /// <summary>
        /// The service query request.
        /// </summary>
        public virtual ServiceQueryRequest ServiceQueryRequest { get; set; }
    }
}