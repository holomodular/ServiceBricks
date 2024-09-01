using System.Reflection;

namespace ServiceBricks.Xunit
{
    public class TestModule : IModule
    {
        public TestModule()
        {
            AutomapperAssemblies = new List<Assembly>()
            {
                typeof(TestModule).Assembly
            };
        }

        public List<IModule> DependentModules { get; set; }

        public List<Assembly> AutomapperAssemblies { get; set; }

        public List<Assembly> ViewAssemblies { get; set; }
    }
}