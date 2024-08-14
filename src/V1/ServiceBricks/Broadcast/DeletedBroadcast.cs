namespace ServiceBricks
{
    /// <summary>
    /// This is a domain event when an object is created.
    /// </summary>
    /// <typeparam name="TObject"></typeparam>
    public partial class DeletedBroadcast<TObject> : DomainBroadcast<TObject>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="obj"></param>
        public DeletedBroadcast(TObject obj)
        {
            DomainObject = obj;
        }
    }
}