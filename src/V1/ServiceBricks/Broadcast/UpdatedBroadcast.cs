namespace ServiceBricks
{
    /// <summary>
    /// This is a domain event when an object is created.
    /// </summary>
    /// <typeparam name="TObject"></typeparam>
    public class UpdatedBroadcast<TObject> : DomainBroadcast<TObject>
    {
        public UpdatedBroadcast(TObject obj)
        {
            DomainObject = obj;
        }
    }
}