namespace ServiceBricks
{
    /// <summary>
    /// This is a domain event to create an application email.
    /// </summary>
    /// <typeparam name="ApplicationSmsDto"></typeparam>
    public partial class CreateApplicationSmsBroadcast : DomainBroadcast<ApplicationSmsDto>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="obj"></param>
        public CreateApplicationSmsBroadcast(ApplicationSmsDto obj)
        {
            DomainObject = obj;
        }
    }
}