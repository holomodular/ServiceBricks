namespace ServiceBricks
{
    /// <summary>
    /// This is a request list.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial interface IRequestList<T> : IRequest, IListOfItems<T>
    {
    }
}