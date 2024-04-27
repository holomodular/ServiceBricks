namespace ServiceBricks
{
    /// <summary>
    /// This is an AJAX response object.
    /// </summary>
    public partial interface IAjaxResponse
    {
        void AddReload();

        void AddRedirectTo(string url);
    }
}