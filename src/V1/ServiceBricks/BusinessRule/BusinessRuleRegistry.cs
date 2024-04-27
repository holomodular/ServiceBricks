namespace ServiceBricks
{
    /// <summary>
    /// This is a registry of all business rules registered in the application.
    /// </summary>
    public partial class BusinessRuleRegistry : RegistryList<Type, Type>, IBusinessRuleRegistry
    {
        public static BusinessRuleRegistry Instance = new BusinessRuleRegistry();
    }
}