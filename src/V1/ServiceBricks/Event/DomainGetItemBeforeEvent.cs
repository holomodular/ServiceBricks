namespace ServiceBricks
{
    /// <summary>
    /// This domain event fires before get
    /// </summary>
    /// <typeparam name="TDomainObject"></typeparam>
    public partial class DomainGetItemBeforeEvent<TDomainObject> : DomainEvent<TDomainObject> where TDomainObject : IDomainObject<TDomainObject>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="obj"></param>
        public DomainGetItemBeforeEvent(TDomainObject obj) : base()
        {
            DomainObject = obj;
        }
    }
}