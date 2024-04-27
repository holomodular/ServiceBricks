namespace ServiceBricks
{
    /// <summary>
    /// This is a domain event when an object is created.
    /// </summary>
    /// <typeparam name="TObject"></typeparam>
    public class DeletedBroadcast<TObject> : DomainBroadcast<TObject>
    {
        public DeletedBroadcast(TObject obj)
        {
            DomainObject = obj;
        }
    }
}