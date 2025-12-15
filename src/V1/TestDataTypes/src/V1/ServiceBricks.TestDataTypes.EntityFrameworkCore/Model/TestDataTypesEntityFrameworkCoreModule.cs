using System.Reflection;

namespace ServiceBricks.TestDataTypes.EntityFrameworkCore
{
    /// <summary>
    /// The module definition for the TestDataTypesEntityFrameworkCore module.
    /// </summary>
    public partial class TestDataTypesEntityFrameworkCoreModule : ServiceBricks.Module
    {
        public static TestDataTypesEntityFrameworkCoreModule Instance = new TestDataTypesEntityFrameworkCoreModule();

        /// <summary>
        /// Constructor for the TestDataTypesEntityFrameworkCore module.
        /// </summary>
        public TestDataTypesEntityFrameworkCoreModule()
        {
            DependentModules = new List<IModule>()
            {
                TestDataTypesModule.Instance
            };
        }
    }
}
