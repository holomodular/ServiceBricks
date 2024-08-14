namespace ServiceBricks
{
    /// <summary>
    /// This property is required for an object.
    /// </summary>
    public partial interface IDpIsActive
    {
        /// <summary>
        /// This indicates if the object is active.
        /// </summary>
        bool IsActive { get; set; }
    }
}