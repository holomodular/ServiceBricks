namespace ServiceBricks
{
    /// <summary>
    /// This is a registry of all business rules registered in the application.
    /// </summary>
    public partial class BusinessRuleRegistry : RegistryList<Type, Type>, IBusinessRuleRegistry
    {
        /// <summary>
        /// Singleton instance of the business rule registry.
        /// </summary>
        public static BusinessRuleRegistry Instance = new BusinessRuleRegistry();
    }
}