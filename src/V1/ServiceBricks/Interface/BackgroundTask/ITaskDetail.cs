namespace ServiceBricks
{
    /// <summary>
    /// This ensures work detail properties are enforced.
    /// </summary>
    public interface ITaskDetail
    { }

    /// <summary>
    /// This ensures work detail properties are enforced.
    /// </summary>
    /// <typeparam name="TWorkDetail"></typeparam>
    /// <typeparam name="TWorker"></typeparam>
    public interface ITaskDetail<TWorkDetail, TWorker> : ITaskDetail
        where TWorkDetail : ITaskDetail<TWorkDetail, TWorker>
        where TWorker : ITaskWork<TWorkDetail, TWorker>
    {
    }
}