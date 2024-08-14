namespace ServiceBricks
{
    /// <summary>
    /// This API event fires before get.
    /// </summary>
    /// <typeparam name="TDomainObject"></typeparam>
    public partial class ApiGetItemBeforeEvent<TDomainObject, TDtoObject> : DomainEvent<TDomainObject>
        where TDomainObject : IDomainObject
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="storageKey"></param>
        /// <param name="domainObject"></param>
        /// <param name="dtoObject"></param>
        public ApiGetItemBeforeEvent(string storageKey, TDomainObject domainObject, TDtoObject dtoObject) : base()
        {
            StorageKey = storageKey;
            DomainObject = domainObject;
            DtoObject = dtoObject;
        }

        /// <summary>
        /// The storage key.
        /// </summary>
        public virtual string StorageKey { get; set; }

        /// <summary>
        /// The data transfer object.
        /// </summary>
        public virtual TDtoObject DtoObject { get; set; }
    }
}