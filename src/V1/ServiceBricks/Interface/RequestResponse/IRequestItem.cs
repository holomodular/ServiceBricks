namespace ServiceBricks
{
    /// <summary>
    /// This is a request item.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial interface IRequestItem<T> : IRequest, IItem<T>
    {
    }
}