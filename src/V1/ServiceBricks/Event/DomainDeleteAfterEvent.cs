namespace ServiceBricks
{
    /// <summary>
    /// This domain event fires after delete.
    /// </summary>
    /// <typeparam name="TDomainObject"></typeparam>
    public partial class DomainDeleteAfterEvent<TDomainObject> : DomainEvent<TDomainObject> where TDomainObject : IDomainObject<TDomainObject>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="obj"></param>
        public DomainDeleteAfterEvent(TDomainObject obj) : base()
        {
            DomainObject = obj;
        }

        /// <summary>
        /// Response
        /// </summary>
        public virtual IResponse Response { get; set; }
    }
}