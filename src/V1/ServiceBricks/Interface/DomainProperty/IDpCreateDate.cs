namespace ServiceBricks
{
    /// <summary>
    /// This property is required for an object.
    /// </summary>
    public partial interface IDpCreateDate
    {
        /// <summary>
        /// The date the object was created.
        /// </summary>
        DateTimeOffset CreateDate { get; set; }
    }
}