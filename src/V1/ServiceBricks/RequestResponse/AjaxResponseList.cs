namespace ServiceBricks
{
    /// <summary>
    /// This is an AJAX response object with a list.
    /// </summary>
    public partial class AjaxResponseList<T> : AjaxResponse, IResponseList<T>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public AjaxResponseList() : base()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="items"></param>
        public AjaxResponseList(List<T> items) : base()
        {
            List = items;
        }

        /// <summary>
        /// The response typed object item.
        /// </summary>
        public virtual List<T> List { get; set; }
    }
}