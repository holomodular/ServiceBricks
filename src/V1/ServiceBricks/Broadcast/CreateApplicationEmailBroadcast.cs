namespace ServiceBricks
{
    /// <summary>
    /// This is a domain event to create an application email.
    /// </summary>
    /// <typeparam name="ApplicationEmailDto"></typeparam>
    public partial class CreateApplicationEmailBroadcast : DomainBroadcast<ApplicationEmailDto>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="obj"></param>
        public CreateApplicationEmailBroadcast(ApplicationEmailDto obj)
        {
            DomainObject = obj;
        }
    }
}