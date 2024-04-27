namespace ServiceBricks
{
    /// <summary>
    /// This is a response item.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial interface IResponseItem<T> : IResponse, IItem<T>
    {
    }
}