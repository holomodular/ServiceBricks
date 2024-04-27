using System.Reflection;

namespace ServiceBricks
{
    public interface IModule
    {
        public List<IModule> DependentModules { get; }
        public List<Assembly> AutomapperAssemblies { get; }
        public List<Assembly> ViewAssemblies { get; }
    }
}