namespace ServiceBricks
{
    /// <summary>
    /// This is a domain event to create an application email.
    /// </summary>
    /// <typeparam name="ApplicationSmsDto"></typeparam>
    public class CreateApplicationSmsBroadcast : DomainBroadcast<ApplicationSmsDto>
    {
        public CreateApplicationSmsBroadcast(ApplicationSmsDto obj)
        {
            DomainObject = obj;
        }
    }
}