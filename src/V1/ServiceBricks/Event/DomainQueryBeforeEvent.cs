using ServiceQuery;

namespace ServiceBricks
{
    /// <summary>
    /// This domain event fires before query
    /// </summary>
    /// <typeparam name="TDomainObject"></typeparam>
    public partial class DomainQueryBeforeEvent<TDomainObject> : DomainEvent<TDomainObject> where TDomainObject : IDomainObject<TDomainObject>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="request"></param>
        public DomainQueryBeforeEvent(ServiceQueryRequest request) : base()
        {
            ServiceQueryRequest = request;
        }

        /// <summary>
        /// The service query request
        /// </summary>
        public virtual ServiceQueryRequest ServiceQueryRequest { get; set; }
    }
}