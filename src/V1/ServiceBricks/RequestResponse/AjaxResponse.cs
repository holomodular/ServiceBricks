namespace ServiceBricks
{
    /// <summary>
    /// This is an AJAX response object.
    /// </summary>
    public partial class AjaxResponse : Response, IAjaxResponse
    {
        /// <summary>
        /// Reload the page.
        /// </summary>
        public const string REDIRECT_RELOAD = "reload";

        /// <summary>
        /// The data with the response.
        /// </summary>
        public virtual object Data { get; set; }

        /// <summary>
        /// The redirect option.
        /// </summary>
        public virtual string Redirect { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public AjaxResponse() : base()
        {
        }

        /// <summary>
        /// Tell the page to reload.
        /// </summary>
        /// <returns></returns>
        public virtual void AddReload()
        {
            this.Redirect = REDIRECT_RELOAD;
        }

        /// <summary>
        /// Tell the page to redirect to a url.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public virtual void AddRedirectTo(string url)
        {
            this.Redirect = url;
        }
    }
}