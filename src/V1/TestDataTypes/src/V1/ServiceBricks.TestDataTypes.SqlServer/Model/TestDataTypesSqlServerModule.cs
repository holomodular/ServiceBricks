using System.Reflection;
using ServiceBricks.TestDataTypes.EntityFrameworkCore;

namespace ServiceBricks.TestDataTypes.SqlServer
{
    /// <summary>
    /// The module definition for the TestDataTypesSqlServer module.
    /// </summary>
    public partial class TestDataTypesSqlServerModule : ServiceBricks.Module
    {
        public static TestDataTypesSqlServerModule Instance = new TestDataTypesSqlServerModule();

        /// <summary>
        /// Constructor for the TestDataTypesSqlServer module.
        /// </summary>
        public TestDataTypesSqlServerModule()
        {
            DependentModules = new List<IModule>()
            {
                TestDataTypesEntityFrameworkCoreModule.Instance
            };
        }
    }
}
