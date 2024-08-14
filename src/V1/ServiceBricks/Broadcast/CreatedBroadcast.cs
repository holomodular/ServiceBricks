namespace ServiceBricks
{
    /// <summary>
    /// This is a domain event when an object is created.
    /// </summary>
    /// <typeparam name="TObject"></typeparam>
    public partial class CreatedBroadcast<TObject> : DomainBroadcast<TObject>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="obj"></param>
        public CreatedBroadcast(TObject obj)
        {
            DomainObject = obj;
        }
    }
}