namespace ServiceBricks
{
    /// <summary>
    /// Represents an object with paging parameters.
    /// </summary>
    public partial interface IPaging
    {
        /// <summary>
        /// The start page.
        /// </summary>
        int PageNumber { get; set; }

        /// <summary>
        /// The number of items per page.
        /// </summary>
        int PageSize { get; set; }
    }
}