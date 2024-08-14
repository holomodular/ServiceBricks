namespace ServiceBricks
{
    /// <summary>
    /// This is a domain event to create an application log.
    /// </summary>
    /// <typeparam name="ApplicationLogDto"></typeparam>
    public partial class CreateApplicationLogBroadcast : DomainBroadcast<ApplicationLogDto>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="obj"></param>
        public CreateApplicationLogBroadcast(ApplicationLogDto obj)
        {
            DomainObject = obj;
        }
    }
}