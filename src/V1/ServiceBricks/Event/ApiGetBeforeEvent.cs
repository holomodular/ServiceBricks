namespace ServiceBricks
{
    /// <summary>
    /// This API event fires before get.
    /// </summary>
    /// <typeparam name="TDomainObject"></typeparam>
    public partial class ApiGetBeforeEvent<TDomainObject, TDtoObject> : DomainEvent<TDomainObject>
        where TDomainObject : IDomainObject
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="storageKey"></param>
        /// <param name="domainObject"></param>
        /// <param name="dtoObject"></param>
        public ApiGetBeforeEvent(string storageKey, TDomainObject domainObject) : base()
        {
            StorageKey = storageKey;
            DomainObject = domainObject;
        }

        /// <summary>
        /// The storage key.
        /// </summary>
        public virtual string StorageKey { get; set; }
    }
}