using System.Reflection;

namespace ServiceBricks.Storage.EntityFrameworkCore.Xunit.Model
{
    public class ExampleModule : IModule
    {
        public ExampleModule()
        {
            AutomapperAssemblies = new List<Assembly>()
            {
                typeof(ExampleModule).Assembly
            };
        }

        public string Name { get; set; }

        public List<IModule> DependentModules { get; set; }

        public List<Assembly> AutomapperAssemblies { get; set; }

        public List<Assembly> ViewAssemblies { get; set; }
    }
}