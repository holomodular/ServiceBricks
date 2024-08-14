namespace ServiceBricks
{
    /// <summary>
    /// This domain event fires before update.
    /// </summary>
    /// <typeparam name="TDomainObject"></typeparam>
    public partial class DomainUpdateBeforeEvent<TDomainObject> : DomainEvent<TDomainObject> where TDomainObject : IDomainObject<TDomainObject>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="obj"></param>
        public DomainUpdateBeforeEvent(TDomainObject obj) : base()
        {
            DomainObject = obj;
        }
    }
}