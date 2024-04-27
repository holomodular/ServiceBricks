namespace ServiceBricks
{
    /// <summary>
    /// This event fires BEFORE getting a domain object.
    /// </summary>
    /// <typeparam name="TDomainObject"></typeparam>
    public partial class ApiGetItemBeforeEvent<TDomainObject, TDtoObject> : DomainEvent<TDomainObject>
        where TDomainObject : IDomainObject
    {
        public ApiGetItemBeforeEvent() : base()
        {
        }

        public ApiGetItemBeforeEvent(string storageKey, TDomainObject domainObject, TDtoObject dtoObject) : base()
        {
            StorageKey = storageKey;
            DomainObject = domainObject;
            DtoObject = dtoObject;
        }

        public string StorageKey { get; set; }

        public TDtoObject DtoObject { get; set; }
    }
}