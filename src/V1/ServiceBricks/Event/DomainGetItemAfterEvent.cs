namespace ServiceBricks
{
    /// <summary>
    /// This domain event fires after get
    /// </summary>
    /// <typeparam name="TDomainObject"></typeparam>
    public partial class DomainGetItemAfterEvent<TDomainObject> : DomainEvent<TDomainObject> where TDomainObject : IDomainObject<TDomainObject>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="obj"></param>
        public DomainGetItemAfterEvent(TDomainObject obj) : base()
        {
            DomainObject = obj;
        }
    }
}