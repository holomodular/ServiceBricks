using ServiceQuery;

namespace ServiceBricks
{
    /// <summary>
    /// This event fires AFTER returning a IQueryable object.
    /// </summary>
    /// <typeparam name="TDomainObject"></typeparam>
    public partial class DomainQueryAfterEvent<TDomainObject> : DomainEvent<TDomainObject> where TDomainObject : IDomainObject<TDomainObject>
    {
        public DomainQueryAfterEvent() : base()
        {
        }

        public DomainQueryAfterEvent(IResponseItem<ServiceQueryResponse<TDomainObject>> resp) : base()
        {
            Response = resp;
        }

        public virtual IResponseItem<ServiceQueryResponse<TDomainObject>> Response { get; set; }
    }
}