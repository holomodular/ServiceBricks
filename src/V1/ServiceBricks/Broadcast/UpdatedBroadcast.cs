namespace ServiceBricks
{
    /// <summary>
    /// This is a domain event when an object is created.
    /// </summary>
    /// <typeparam name="TObject"></typeparam>
    public partial class UpdatedBroadcast<TObject> : DomainBroadcast<TObject>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="obj"></param>
        public UpdatedBroadcast(TObject obj)
        {
            DomainObject = obj;
        }
    }
}