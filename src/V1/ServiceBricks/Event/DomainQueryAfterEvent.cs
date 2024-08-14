using ServiceQuery;

namespace ServiceBricks
{
    /// <summary>
    /// This domain event fires after query
    /// </summary>
    /// <typeparam name="TDomainObject"></typeparam>
    public partial class DomainQueryAfterEvent<TDomainObject> : DomainEvent<TDomainObject> where TDomainObject : IDomainObject<TDomainObject>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="resp"></param>
        public DomainQueryAfterEvent(IResponseItem<ServiceQueryResponse<TDomainObject>> resp) : base()
        {
            Response = resp;
        }

        /// <summary>
        /// The response
        /// </summary>
        public virtual IResponseItem<ServiceQueryResponse<TDomainObject>> Response { get; set; }
    }
}