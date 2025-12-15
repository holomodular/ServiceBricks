using ServiceBricks.Xunit;

namespace MigrationsHost
{
    internal class Program
    {
        public static ISystemManager SystemManager { get; set; }

        private static void Main(string[] args)
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(StartupMigrations));
        }
    }
}