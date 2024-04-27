namespace ServiceBricks
{
    /// <summary>
    /// This is a domain event to create an application log.
    /// </summary>
    /// <typeparam name="ApplicationLogDto"></typeparam>
    public class CreateApplicationLogBroadcast : DomainBroadcast<ApplicationLogDto>
    {
        public CreateApplicationLogBroadcast(ApplicationLogDto obj)
        {
            DomainObject = obj;
        }
    }
}