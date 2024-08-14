namespace ServiceBricks
{
    /// <summary>
    /// This is the worker process interface.
    /// </summary>
    public partial interface ITaskWork
    { }

    /// <summary>
    /// This provides the ability to do work.
    /// </summary>
    /// <typeparam name="TWorkDetail"></typeparam>
    /// <typeparam name="TWorker"></typeparam>
    public partial interface ITaskWork<TWorkDetail, TWorker> : ITaskWork
        where TWorkDetail : ITaskDetail<TWorkDetail, TWorker>
        where TWorker : ITaskWork<TWorkDetail, TWorker>
    {
        /// <summary>
        /// This is the work to be done.
        /// </summary>
        /// <param name="detail"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task DoWork(TWorkDetail detail, CancellationToken cancellationToken);
    }

    /// <summary>
    /// This provides the ability to do work.
    /// </summary>
    /// <typeparam name="TWorkDetail"></typeparam>
    /// <typeparam name="TWorker"></typeparam>
    /// <typeparam name="TDomainObject"></typeparam>
    public partial interface ITaskWork<TWorkDetail, TWorker, TDomainObject> : ITaskWork<TWorkDetail, TWorker>, ITaskWork
    where TWorkDetail : ITaskDetail<TWorkDetail, TWorker>
    where TWorker : ITaskWork<TWorkDetail, TWorker>
    {
    }
}