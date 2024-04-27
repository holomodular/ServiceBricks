namespace ServiceBricks
{
    /// <summary>
    /// This is a domain event when an object is created.
    /// </summary>
    /// <typeparam name="TObject"></typeparam>
    public class CreatedBroadcast<TObject> : DomainBroadcast<TObject>
    {
        public CreatedBroadcast(TObject obj)
        {
            DomainObject = obj;
        }
    }
}