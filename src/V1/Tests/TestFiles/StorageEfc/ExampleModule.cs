using System.Reflection;

namespace ServiceBricks.Storage.EntityFrameworkCore.Xunit.Model
{
    public class ExampleModule : ServiceBricks.Module
    {
        public ExampleModule()
        {
            AutomapperAssemblies = new List<Assembly>()
            {
                typeof(ExampleModule).Assembly
            };
        }
    }
}