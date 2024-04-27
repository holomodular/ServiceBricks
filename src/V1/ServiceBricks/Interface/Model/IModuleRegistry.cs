namespace ServiceBricks
{
    public interface IModuleRegistry : IRegistry<Type, IModule>
    {
        List<IModule> GetModules();
    }
}