using ServiceQuery;

namespace ServiceBricks
{
    /// <summary>
    /// This event fires BEFORE returning a IQueryable object.
    /// </summary>
    /// <typeparam name="TDomainObject"></typeparam>
    public partial class DomainQueryBeforeEvent<TDomainObject> : DomainEvent<TDomainObject> where TDomainObject : IDomainObject<TDomainObject>
    {
        public DomainQueryBeforeEvent() : base()
        {
        }

        public DomainQueryBeforeEvent(ServiceQueryRequest request) : base()
        {
            ServiceQueryRequest = request;
        }

        public ServiceQueryRequest ServiceQueryRequest { get; set; }
    }
}