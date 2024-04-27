namespace ServiceBricks
{
    /// <summary>
    /// This event fires BEFORE getting all domain objects.
    /// </summary>
    /// <typeparam name="TDomainObject"></typeparam>
    public partial class DomainGetAllBeforeEvent<TDomainObject> : DomainEvent<TDomainObject> where TDomainObject : IDomainObject<TDomainObject>
    {
        public DomainGetAllBeforeEvent() : base()
        {
        }
    }
}