namespace ServiceBricks
{
    /// <summary>
    /// This is a domain event to create an application email.
    /// </summary>
    /// <typeparam name="ApplicationEmailDto"></typeparam>
    public class CreateApplicationEmailBroadcast : DomainBroadcast<ApplicationEmailDto>
    {
        public CreateApplicationEmailBroadcast(ApplicationEmailDto obj)
        {
            DomainObject = obj;
        }
    }
}