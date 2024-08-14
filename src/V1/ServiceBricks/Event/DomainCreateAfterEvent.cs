namespace ServiceBricks
{
    /// <summary>
    /// This domain event fires after create
    /// </summary>
    /// <typeparam name="TDomainObject"></typeparam>
    public partial class DomainCreateAfterEvent<TDomainObject> : DomainEvent<TDomainObject> where TDomainObject : IDomainObject<TDomainObject>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="obj"></param>
        public DomainCreateAfterEvent(TDomainObject obj) : base()
        {
            DomainObject = obj;
        }
    }
}