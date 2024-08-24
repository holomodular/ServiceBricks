namespace ServiceBricks
{
    /// <summary>
    /// This domain event fires before get
    /// </summary>
    /// <typeparam name="TDomainObject"></typeparam>
    public partial class DomainGetBeforeEvent<TDomainObject> : DomainEvent<TDomainObject> where TDomainObject : IDomainObject<TDomainObject>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="obj"></param>
        public DomainGetBeforeEvent(TDomainObject obj) : base()
        {
            DomainObject = obj;
        }
    }
}