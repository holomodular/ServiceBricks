namespace ServiceBricks
{
    /// <summary>
    /// This domain event fires after update
    /// </summary>
    /// <typeparam name="TDomainObject"></typeparam>
    public partial class DomainUpdateAfterEvent<TDomainObject> : DomainEvent<TDomainObject> where TDomainObject : IDomainObject<TDomainObject>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="obj"></param>
        public DomainUpdateAfterEvent(TDomainObject obj) : base()
        {
            DomainObject = obj;
        }
    }
}