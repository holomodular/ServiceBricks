using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ServiceQuery;

namespace ServiceBricks
{
    /// <summary>
    /// This is a service for processing a domain object table like a queue.
    /// </summary>
    /// <typeparam name="TDomainObject"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public abstract partial class DomainObjectProcessQueueService<TDomainObject>
        where TDomainObject : class, IDomainObject<TDomainObject>, IDpProcessQueue
    {
        /// <summary>
        /// Default Value.
        /// </summary>
        public const int BATCH_NUMBER_TO_PROCESS = 1;

        /// <summary>
        /// Default Value.
        /// </summary>
        public const bool CONTINUE_PROCESSING_ON_ERROR = true;

        /// <summary>
        /// Default Value.
        /// </summary>
        public const bool PROCESS_FAILED_MESSAGES = true;

        /// <summary>
        /// Default Value.
        /// </summary>
        public const bool FIX_ORPHANED_RECORDS = true;

        /// <summary>
        /// Default Value.
        /// </summary>
        public TimeSpan ORPHANED_RECORD_TIMEOUT = TimeSpan.FromMinutes(10);

        /// <summary>
        /// Default value.
        /// </summary>
        public const int RETRY_NUMBER = 10;

        /// <summary>
        /// Default Value.
        /// </summary>
        public TimeSpan FAILED_RETRY_INTERVAL = TimeSpan.FromMinutes(5);

        /// <summary>
        /// Internal.
        /// </summary>
        protected readonly IDomainObjectProcessQueueStorageRepository<TDomainObject> _repository;

        /// <summary>
        /// Internal.
        /// </summary>
        private readonly ILogger<DomainObjectProcessQueueService<TDomainObject>> _logger;

        /// <summary>
        /// Consrtuctor.
        /// </summary>
        /// <param name="loggerFactory"></param>
        /// <param name="repository"></param>
        public DomainObjectProcessQueueService(
            ILoggerFactory loggerFactory,
            IDomainObjectProcessQueueStorageRepository<TDomainObject> repository)
        {
            _logger = loggerFactory.CreateLogger<DomainObjectProcessQueueService<TDomainObject>>();
            _repository = repository;
            NumberToBatchProcess = BATCH_NUMBER_TO_PROCESS;
            AllowErrorContinueProcessing = CONTINUE_PROCESSING_ON_ERROR;
            RetryFailedItems = PROCESS_FAILED_MESSAGES;
            RetryCount = RETRY_NUMBER;
            RetryFailedInterval = FAILED_RETRY_INTERVAL;
            FixOrphanedProcessingRecords = FIX_ORPHANED_RECORDS;
            OrphanedProcessingTimeout = ORPHANED_RECORD_TIMEOUT;
        }

        /// <summary>
        /// If true, the process will continue to process new messages on the queue. If false, will stop processing messages when an error happens.
        /// </summary>
        public virtual bool AllowErrorContinueProcessing { get; set; }

        /// <summary>
        /// Number of items to pickup in a batch to process.
        /// </summary>
        public virtual int NumberToBatchProcess { get; set; }

        /// <summary>
        /// After pickuping up new messages and processing them, go back and re-execute errors
        /// </summary>
        public virtual bool RetryFailedItems { get; set; }

        /// <summary>
        /// The number of times to retry a failed item before bypassing it.
        /// </summary>
        public virtual int RetryCount { get; set; }

        /// <summary>
        /// The time that must pass before we pickup error messages to retry them.
        /// </summary>
        public virtual TimeSpan RetryFailedInterval { get; set; }

        /// <summary>
        /// Fix records marked as CurretlyProcessing that have timed out. May happen on hard application failure.
        /// </summary>
        public virtual bool FixOrphanedProcessingRecords { get; set; }

        /// <summary>
        /// Time that the process should have completed by and designated as orphaned.
        /// </summary>
        public virtual TimeSpan OrphanedProcessingTimeout { get; set; }

        /// <summary>
        /// Execute the process.
        /// </summary>
        /// <param name="cancellationToken"></param>
        public virtual async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            try
            {
                IResponseList<TDomainObject> respItems;
                bool processingErrors = false; //Process new records then errors

                if (FixOrphanedProcessingRecords)
                    await FixOrphanedRecords();

                //Main loop
                while (!cancellationToken.IsCancellationRequested)
                {
                    if (!AllowErrorContinueProcessing)
                    {
                        //Get batch set including errors
                        processingErrors = true;
                        respItems = await _repository.GetQueueItemsAsync(NumberToBatchProcess, true, DateTimeOffset.UtcNow.Subtract(RetryFailedInterval));
                    }
                    else
                    {
                        //Get batch DOES NOT include errors. We are going to process all new messages first, then go back and execute errors that have already happened.
                        if (processingErrors)
                            respItems = await _repository.GetQueueItemsAsync(NumberToBatchProcess, true, DateTimeOffset.UtcNow.Subtract(RetryFailedInterval));
                        else
                            respItems = await _repository.GetQueueItemsAsync(NumberToBatchProcess, false, DateTimeOffset.UtcNow.Subtract(RetryFailedInterval));
                    }

                    //Cancellation requested, make sure records are marked appropriately
                    if (cancellationToken.IsCancellationRequested)
                    {
                        foreach (var item in respItems.List)
                        {
                            item.IsProcessing = false;
                            item.ProcessDate = DateTimeOffset.UtcNow;
                            await _repository.UpdateAsync(item);
                        }
                        return;
                    }

                    if (!respItems.Success)
                        return;

                    if (respItems.List == null || respItems.List.Count == 0)
                    {
                        if (processingErrors) //We completed success and error processing
                            return;

                        //Try to start processing errors
                        processingErrors = true;
                        if (!RetryFailedItems)
                            return;
                        continue; //restart
                    }

                    //Process each record
                    foreach (var item in respItems.List)
                    {
                        //Cancellation requested, make sure remaining records are marked appropriately
                        if (cancellationToken.IsCancellationRequested)
                        {
                            item.IsProcessing = false;
                            item.ProcessDate = DateTimeOffset.UtcNow;
                            await _repository.UpdateAsync(item);
                            continue;
                        }

                        //Next item is an error and we don't process anymore
                        if (item.IsError)
                        {
                            if (!AllowErrorContinueProcessing)
                            {
                                _logger.LogWarning(LocalizationResource.WARNING_BUSINESS_QUEUE_PROCESSOR_STOPPED);
                                return;
                            }

                            if (!RetryFailedItems)
                                return;

                            if ((item.RetryCount + 1) > RetryCount)
                            {
                                item.IsProcessing = false;
                                item.ProcessDate = DateTimeOffset.UtcNow;
                                item.IsComplete = true;
                                await _repository.UpdateAsync(item);
                                continue;
                            }
                            item.RetryCount++;
                        }

                        //Actually do the process
                        await DoProcessingForItem(item);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        private async Task FixOrphanedRecords()
        {
            DateTimeOffset timeoutDate = DateTimeOffset.UtcNow.Subtract(OrphanedProcessingTimeout);
            ServiceQueryRequestBuilder queryBuilder = new ServiceQueryRequestBuilder();
            queryBuilder
                .IsEqual(nameof(IDpProcessQueue.IsComplete), false.ToString())
                .And()
                .IsEqual(nameof(IDpProcessQueue.IsProcessing), true.ToString())
                .And()
                .IsLessThanOrEqual(nameof(IDpProcessQueue.ProcessDate), timeoutDate.ToString());
            var respQ = await _repository.QueryAsync(queryBuilder.Build());
            foreach (var item in respQ.Item.List)
            {
                item.IsProcessing = false;
                item.ProcessDate = DateTimeOffset.UtcNow;
                await _repository.UpdateAsync(item);
            }
        }

        private async Task DoProcessingForItem(TDomainObject item)
        {
            item.ProcessDate = DateTimeOffset.UtcNow;

            try
            {
                //try to process the item
                var respProcess = await ProcessItemAsync(item);
                item.ProcessResponse = JsonConvert.SerializeObject(respProcess);
                if (respProcess.Success)
                {
                    item.IsError = false;
                    item.IsComplete = true;
                }
                else
                {
                    item.IsError = true;
                    if ((item.RetryCount + 1) > RetryCount)
                        item.IsComplete = true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                Response respError = new Response();
                respError.AddMessage(ResponseMessage.CreateError(ex, LocalizationResource.ERROR_BUSINESS_QUEUE_PROCESSOR));
                item.ProcessResponse = JsonConvert.SerializeObject(respError);
                item.IsError = true;
                if ((item.RetryCount + 1) > RetryCount)
                    item.IsComplete = true;
            }
            finally
            {
                item.IsProcessing = false;
                await _repository.UpdateAsync(item);
            }
        }

        /// <summary>
        /// Process the item.
        /// </summary>
        /// <param name="domainObject"></param>
        /// <returns></returns>
        public abstract Task<IResponse> ProcessItemAsync(TDomainObject domainObject);

        public static ServiceQueryRequest GetQueueItemsQuery(int batchNumberToTake, bool pickupErrors, DateTimeOffset errorPickupCutoffDate)
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;

            ServiceQueryRequestBuilder query = new ServiceQueryRequestBuilder();
            query.IsEqual(nameof(IDpProcessQueue.IsComplete), false.ToString())
                .And()
                .IsEqual(nameof(IDpProcessQueue.IsProcessing), false.ToString())
                .And()
                .IsLessThanOrEqual(nameof(IDpProcessQueue.FutureProcessDate), now.ToString("o"));
            if (pickupErrors)
            {
                query.And()
                .IsEqual(nameof(IDpProcessQueue.IsError), true.ToString())
                .And()
                .IsLessThanOrEqual(nameof(IDpProcessQueue.ProcessDate), errorPickupCutoffDate.ToString("o"));
            }
            else
            {
                query.And()
                .IsEqual(nameof(IDpProcessQueue.IsError), false.ToString());
            }
            query.Sort(nameof(IDpProcessQueue.CreateDate), true);
            query.Paging(1, batchNumberToTake, false);
            return query.Build();
        }
    }
}