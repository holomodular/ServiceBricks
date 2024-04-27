using ServiceQuery;

namespace ServiceBricks
{
    /// <summary>
    /// This event fires BEFORE returning a IQueryable object.
    /// </summary>
    /// <typeparam name="TDto"></typeparam>
    public partial class ApiQueryBeforeEvent<TDto> : DomainEvent<TDto>
        where TDto : IDataTransferObject
    {
        public ApiQueryBeforeEvent() : base()
        {
        }

        public ApiQueryBeforeEvent(ServiceQueryRequest request) : base()
        {
            ServiceQueryRequest = request;
        }

        public ServiceQueryRequest ServiceQueryRequest { get; set; }
    }
}