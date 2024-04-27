namespace ServiceBricks.Xunit
{
    public interface ISystemManager
    {
        //void StartSystem(Type startupType);

        //void StopSystem();

        IServiceProvider ServiceProvider { get; }
    }
}