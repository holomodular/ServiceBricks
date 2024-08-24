namespace ServiceBricks
{
    /// <summary>
    /// This domain event fires after get
    /// </summary>
    /// <typeparam name="TDomainObject"></typeparam>
    public partial class DomainGetAfterEvent<TDomainObject> : DomainEvent<TDomainObject> where TDomainObject : IDomainObject<TDomainObject>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="obj"></param>
        public DomainGetAfterEvent(TDomainObject obj) : base()
        {
            DomainObject = obj;
        }
    }
}