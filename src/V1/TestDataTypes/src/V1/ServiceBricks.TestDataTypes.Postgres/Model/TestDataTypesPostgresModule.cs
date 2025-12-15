using System.Reflection;
using ServiceBricks.TestDataTypes.EntityFrameworkCore;

namespace ServiceBricks.TestDataTypes.Postgres
{
    /// <summary>
    /// The module definition for the TestDataTypesPostgres module.
    /// </summary>
    public partial class TestDataTypesPostgresModule : ServiceBricks.Module
    {
        public static TestDataTypesPostgresModule Instance = new TestDataTypesPostgresModule();

        /// <summary>
        /// Constructor for the TestDataTypesPostgres module.
        /// </summary>
        public TestDataTypesPostgresModule()
        {
            DependentModules = new List<IModule>()
            {
                TestDataTypesEntityFrameworkCoreModule.Instance
            };
        }
    }
}
