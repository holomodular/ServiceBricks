namespace ServiceBricks
{
    /// <summary>
    /// This is an AJAX response object with an item.
    /// </summary>
    public partial class AjaxResponseItem<T> : AjaxResponse, IResponseItem<T>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public AjaxResponseItem() : base()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="item"></param>
        public AjaxResponseItem(T item) : base()
        {
            Item = item;
        }

        /// <summary>
        /// The object item.
        /// </summary>
        public virtual T Item { get; set; }
    }
}