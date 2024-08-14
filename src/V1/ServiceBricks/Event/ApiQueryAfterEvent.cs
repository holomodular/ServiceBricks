using ServiceQuery;

namespace ServiceBricks
{
    /// <summary>
    /// This API event fires after query
    /// </summary>
    /// <typeparam name="TDomainObject"></typeparam>
    public partial class ApiQueryAfterEvent<TDtoObject> : DomainEvent<TDtoObject>
        where TDtoObject : IDataTransferObject
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="response"></param>
        public ApiQueryAfterEvent(ServiceQueryResponse<TDtoObject> response)
        {
            Response = response;
        }

        /// <summary>
        /// Response
        /// </summary>
        public virtual ServiceQueryResponse<TDtoObject> Response { get; set; }
    }
}