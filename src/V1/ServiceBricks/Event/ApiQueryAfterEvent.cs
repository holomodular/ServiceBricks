using ServiceQuery;

namespace ServiceBricks
{
    /// <summary>
    /// This event fires AFTER returning a IQueryable object.
    /// </summary>
    /// <typeparam name="TDomainObject"></typeparam>
    public partial class ApiQueryAfterEvent<TDtoObject> : DomainEvent<TDtoObject>
        where TDtoObject : IDataTransferObject
    {
        public ApiQueryAfterEvent() : base()
        {
        }

        public ApiQueryAfterEvent(ServiceQueryResponse<TDtoObject> response)
        {
            Response = response;
        }

        public virtual ServiceQueryResponse<TDtoObject> Response { get; set; }
    }
}