namespace ServiceBricks
{
    /// <summary>
    /// This property is required for an object.
    /// </summary>
    public partial interface IDpUpdateDate
    {
        /// <summary>
        /// The date the object was last updated.
        /// </summary>
        DateTimeOffset UpdateDate { get; set; }
    }
}