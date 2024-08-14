namespace ServiceBricks
{
    /// <summary>
    /// This property is required for an object.
    /// </summary>
    public partial interface IDpName
    {
        /// <summary>
        /// The name of the object.
        /// </summary>
        string Name { get; set; }
    }
}