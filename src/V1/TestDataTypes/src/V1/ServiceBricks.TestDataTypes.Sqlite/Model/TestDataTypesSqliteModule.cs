using System.Reflection;
using ServiceBricks.TestDataTypes.EntityFrameworkCore;

namespace ServiceBricks.TestDataTypes.Sqlite
{
    /// <summary>
    /// The module definition for the TestDataTypesSqlite module.
    /// </summary>
    public partial class TestDataTypesSqliteModule : ServiceBricks.Module
    {
        public static TestDataTypesSqliteModule Instance = new TestDataTypesSqliteModule();

        /// <summary>
        /// Constructor for the TestDataTypesSqlite module.
        /// </summary>
        public TestDataTypesSqliteModule()
        {
            DependentModules = new List<IModule>()
            {
                TestDataTypesEntityFrameworkCoreModule.Instance
            };
        }
    }
}
