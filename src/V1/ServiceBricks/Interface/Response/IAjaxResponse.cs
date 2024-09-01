namespace ServiceBricks
{
    /// <summary>
    /// This is an AJAX response object.
    /// </summary>
    public partial interface IAjaxResponse
    {
        /// <summary>
        /// Denotes if the page should be reloaded.
        /// </summary>
        void AddReload();

        /// <summary>
        /// Denotes if the page should be redirected to the specified URL.
        /// </summary>
        /// <param name="url"></param>
        void AddRedirectTo(string url);
    }
}