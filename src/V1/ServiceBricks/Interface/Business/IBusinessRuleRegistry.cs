namespace ServiceBricks
{
    /// <summary>
    /// This is a registry of all business rules registered in the application.
    /// </summary>
    public partial interface IBusinessRuleRegistry : IBusinessRuleRegistryList<Type, Type>
    {
    }
}