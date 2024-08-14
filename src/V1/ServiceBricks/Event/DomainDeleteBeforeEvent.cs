namespace ServiceBricks
{
    /// <summary>
    /// This domain event fires before delete
    /// </summary>
    /// <typeparam name="TDomainObject"></typeparam>
    public partial class DomainDeleteBeforeEvent<TDomainObject> : DomainEvent<TDomainObject> where TDomainObject : IDomainObject<TDomainObject>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="obj"></param>
        public DomainDeleteBeforeEvent(TDomainObject obj) : base()
        {
            DomainObject = obj;
        }
    }
}