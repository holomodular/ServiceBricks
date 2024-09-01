namespace ServiceBricks
{
    /// <summary>
    /// Paging records.
    /// </summary>
    public partial class Paging : IPaging
    {
        /// <summary>
        /// The page number.
        /// </summary>
        public virtual int PageNumber { get; set; }

        /// <summary>
        /// The page size.
        /// </summary>
        public virtual int PageSize { get; set; }
    }
}