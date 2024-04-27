namespace ServiceBricks
{
    public partial class ServiceBusRuleRegistration<TBroadcast> : DomainEvent<TBroadcast>
    {
        public ServiceBusRuleRegistration(TBroadcast obj) : base()
        {
            DomainObject = obj;
        }
    }
}