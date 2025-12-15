using System.Reflection;
using ServiceBricks.TestDataTypes.EntityFrameworkCore;

namespace ServiceBricks.TestDataTypes.InMemory
{
    /// <summary>
    /// The module definition for the TestDataTypesInMemory module.
    /// </summary>
    public partial class TestDataTypesInMemoryModule : ServiceBricks.Module
    {
        public static TestDataTypesInMemoryModule Instance = new TestDataTypesInMemoryModule();

        /// <summary>
        /// Constructor for the TestDataTypesInMemory module.
        /// </summary>
        public TestDataTypesInMemoryModule()
        {
            DependentModules = new List<IModule>()
            {
                TestDataTypesEntityFrameworkCoreModule.Instance
            };
        }
    }
}
