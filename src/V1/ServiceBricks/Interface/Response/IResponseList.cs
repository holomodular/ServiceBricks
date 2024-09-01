namespace ServiceBricks
{
    /// <summary>
    /// This is a response list.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial interface IResponseList<T> : IResponse, IListOfItems<T>
    {
    }
}