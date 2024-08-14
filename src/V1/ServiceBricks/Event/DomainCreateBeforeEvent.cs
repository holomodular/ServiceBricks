namespace ServiceBricks
{
    /// <summary>
    /// This domain event fires before create
    /// </summary>
    /// <typeparam name="TDomainObject"></typeparam>
    public partial class DomainCreateBeforeEvent<TDomainObject> : DomainEvent<TDomainObject> where TDomainObject : IDomainObject<TDomainObject>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="obj"></param>
        public DomainCreateBeforeEvent(TDomainObject obj) : base()
        {
            DomainObject = obj;
        }
    }
}