using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ServiceQuery;

namespace ServiceBricks
{
    /// <summary>
    /// This is a service for processing a domain object table like a work queue.
    /// </summary>
    /// <typeparam name="TDomainObject"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public abstract partial class WorkService<TDto>
        where TDto : class, IDataTransferObject, IDpWorkProcess
    {
        protected readonly ILogger<WorkService<TDto>> _logger;
        protected readonly IApiService<TDto> _apiService;

        /// <summary>
        /// Default Value.
        /// </summary>
        public const int BATCH_NUMBER_TO_PROCESS = 10;

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
        public static TimeSpan ORPHANED_RECORD_TIMEOUT = TimeSpan.FromMinutes(10);

        /// <summary>
        /// Default value.
        /// </summary>
        public const int RETRY_NUMBER = 10;

        /// <summary>
        /// Default Value.
        /// </summary>
        public static TimeSpan FAILED_RETRY_INTERVAL = TimeSpan.FromMinutes(5);

        /// <summary>
        /// Consrtuctor.
        /// </summary>
        /// <param name="loggerFactory"></param>
        /// <param name="apiService"></param>
        public WorkService(
            ILoggerFactory loggerFactory,
            IApiService<TDto> apiService)
        {
            _logger = loggerFactory.CreateLogger<WorkService<TDto>>();
            _apiService = apiService;
            NumberToBatchProcess = BATCH_NUMBER_TO_PROCESS;
            RetryFailedItems = PROCESS_FAILED_MESSAGES;
            RetryCount = RETRY_NUMBER;
            RetryFailedInterval = FAILED_RETRY_INTERVAL;
            FixOrphanedProcessingRecords = FIX_ORPHANED_RECORDS;
            OrphanedProcessingTimeout = ORPHANED_RECORD_TIMEOUT;
        }

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
        /// Process the item.
        /// </summary>
        /// <param name="domainObject"></param>
        /// <returns></returns>
        public abstract Task<IResponse> ProcessItemAsync(TDto dto);

        /// <summary>
        /// Execute the process.
        /// </summary>
        /// <param name="cancellationToken"></param>
        public virtual async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            try
            {
                IResponseList<TDto> respItems;
                bool processingErrors = false; //Process new records then errors

                if (FixOrphanedProcessingRecords)
                    await FixOrphanedRecords();

                // Main loop
                while (!cancellationToken.IsCancellationRequested)
                {
                    // We are going to process all new messages first, then go back and execute errors that have already happened.
                    if (processingErrors)
                        respItems = await GetQueueItemsAsync(NumberToBatchProcess, true, DateTimeOffset.UtcNow.Subtract(RetryFailedInterval));
                    else
                        respItems = await GetQueueItemsAsync(NumberToBatchProcess, false, DateTimeOffset.UtcNow.Subtract(RetryFailedInterval));

                    if (respItems.Error)
                        return;

                    if (respItems.List == null || respItems.List.Count == 0)
                    {
                        if (processingErrors) //We completed success and error processing
                            return;

                        // Try to start processing errors
                        processingErrors = true;
                        if (!RetryFailedItems)
                            return;
                        continue; //restart
                    }

                    // Process each record
                    foreach (var item in respItems.List)
                    {
                        //Cancellation requested, make sure remaining records are marked appropriately
                        if (cancellationToken.IsCancellationRequested)
                        {
                            item.IsProcessing = false;
                            item.ProcessDate = DateTimeOffset.UtcNow;
                            await _apiService.UpdateAsync(item);
                            continue;
                        }

                        // Actually do the process
                        await DoProcessingForItemAsync(item);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        protected virtual async Task FixOrphanedRecords()
        {
            DateTimeOffset timeoutDate = DateTimeOffset.UtcNow.Subtract(OrphanedProcessingTimeout);
            ServiceQueryRequestBuilder queryBuilder = new ServiceQueryRequestBuilder();
            queryBuilder
                .IsEqual(nameof(IDpWorkProcess.IsComplete), false.ToString())
                .And()
                .IsEqual(nameof(IDpWorkProcess.IsProcessing), true.ToString())
                .And()
                .IsLessThanOrEqual(nameof(IDpWorkProcess.ProcessDate), timeoutDate.ToString("o"));
            var respQ = await _apiService.QueryAsync(queryBuilder.Build());
            if (respQ.Success && respQ.Item.List.Count > 0)
            {
                foreach (var item in respQ.Item.List)
                {
                    item.IsProcessing = false;
                    item.ProcessDate = DateTimeOffset.UtcNow;
                    await _apiService.UpdateAsync(item);
                }
            }
        }

        protected virtual async Task DoProcessingForItemAsync(TDto item)
        {
            item.ProcessDate = DateTimeOffset.UtcNow;
            if (item.IsError)
            {
                // Should not process this more than retry allows
                if ((item.RetryCount + 1) > RetryCount)
                {
                    item.IsProcessing = false;
                    item.IsComplete = true;
                    await _apiService.UpdateAsync(item);
                    return;
                }

                // increment retry count
                item.RetryCount++;
            }

            try
            {
                //try to process the item
                var respProcess = await ProcessItemAsync(item);

                // Save its response
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
                await _apiService.UpdateAsync(item);
            }
        }

        /// <summary>
        /// Get the list of queue items to process
        /// </summary>
        /// <param name="batchNumberToTake"></param>
        /// <param name="pickupErrors"></param>
        /// <param name="errorPickupCutoffDate"></param>
        /// <returns></returns>
        public virtual async Task<IResponseList<TDto>> GetQueueItemsAsync(int batchNumberToTake, bool pickupErrors, DateTimeOffset errorPickupCutoffDate)
        {
            ResponseList<TDto> response = new ResponseList<TDto>();
            try
            {
                // Check if records are available
                var query = GetQueueItemsQuery(
                    batchNumberToTake,
                    pickupErrors,
                    errorPickupCutoffDate);
                DateTimeOffset now = DateTimeOffset.UtcNow;
                var respQuery = await _apiService.QueryAsync(query);
                response.CopyFrom(respQuery);
                if (response.Error || respQuery.Item == null || respQuery.Item.List.Count == 0)
                    return response;

                foreach (var item in respQuery.Item.List)
                {
                    item.IsProcessing = true;
                    item.ProcessDate = now;
                    var respUpdate = await _apiService.UpdateAsync(item);
                    if (respUpdate.Success)
                        response.List.Add(respUpdate.Item);
                }
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_STORAGE));
            }
            return response;
        }

        /// <summary>
        /// Get the query to pickup items from the queue.
        /// </summary>
        /// <param name="batchNumberToTake"></param>
        /// <param name="pickupErrors"></param>
        /// <param name="errorPickupCutoffDate"></param>
        /// <returns></returns>
        protected virtual ServiceQueryRequest GetQueueItemsQuery(int batchNumberToTake, bool pickupErrors, DateTimeOffset errorPickupCutoffDate)
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;

            ServiceQueryRequestBuilder query = new ServiceQueryRequestBuilder();
            query.IsEqual(nameof(IDpWorkProcess.IsComplete), false.ToString())
                .And()
                .IsEqual(nameof(IDpWorkProcess.IsProcessing), false.ToString())
                .And()
                .IsLessThanOrEqual(nameof(IDpWorkProcess.FutureProcessDate), now.ToString("o"));
            if (pickupErrors)
            {
                query.And()
                .IsEqual(nameof(IDpWorkProcess.IsError), true.ToString())
                .And()
                .IsLessThanOrEqual(nameof(IDpWorkProcess.ProcessDate), errorPickupCutoffDate.ToString("o"));
            }
            else
            {
                query.And()
                .IsEqual(nameof(IDpWorkProcess.IsError), false.ToString());
            }
            query.SortAsc(nameof(IDpWorkProcess.CreateDate));
            query.Paging(1, batchNumberToTake, false);
            return query.Build();
        }
    }
}