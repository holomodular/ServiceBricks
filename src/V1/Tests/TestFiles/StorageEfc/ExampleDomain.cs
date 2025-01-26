using System.Linq.Expressions;

namespace ServiceBricks.Storage.EntityFrameworkCore.Xunit
{
    public class ExampleDomain : EntityFrameworkCoreDomainObject<ExampleDomain>, IDpCreateDate, IDpUpdateDate
    {
        public int Key { get; set; }
        public string Name { get; set; }

        public DateTimeOffset CreateDate { get; set; }
        public DateTimeOffset UpdateDate { get; set; }
        public DateTimeOffset ExampleDate { get; set; }
        public DateTimeOffset? ExampleNullableDate { get; set; }
        public DateTimeOffset? ExampleNullableDateNotSet { get; set; }
        public DateTime SimpleDate { get; set; }
        public DateTime SimpleNullableDate { get; set; }
        public DateTime? SimpleNullableDateNotSet { get; set; }

        public override Expression<Func<ExampleDomain, bool>> DomainGetItemFilter(ExampleDomain obj)
        {
            return x => x.Key == obj.Key;
        }
    }

    public class ExampleWorkProcessDomain : EntityFrameworkCoreDomainObject<ExampleWorkProcessDomain>, IDpWorkProcess
    {
        public int Key { get; set; }
        public string Name { get; set; }

        /// <summary>
        /// Determine if processing encountered an error.
        /// </summary>
        public bool IsError { get; set; }

        /// <summary>
        /// Determine if processing has completed.
        /// </summary>
        public bool IsComplete { get; set; }

        /// <summary>
        /// The current number of retries.
        /// </summary>
        public int RetryCount { get; set; }

        /// <summary>
        /// The future process date.
        /// </summary>
        public DateTimeOffset FutureProcessDate { get; set; }

        /// <summary>
        /// The date and time the message was created.
        /// </summary>
        public DateTimeOffset CreateDate { get; set; }

        /// <summary>
        /// The date and time the message was last updated.
        /// </summary>
        public DateTimeOffset UpdateDate { get; set; }

        /// <summary>
        /// The date and time the message was processed.
        /// </summary>
        public DateTimeOffset ProcessDate { get; set; }

        /// <summary>
        /// The processing response.
        /// </summary>
        public string ProcessResponse { get; set; }

        /// <summary>
        /// Determine if the message is currently processing.
        /// </summary>
        public bool IsProcessing { get; set; }

        public override Expression<Func<ExampleWorkProcessDomain, bool>> DomainGetItemFilter(ExampleWorkProcessDomain obj)
        {
            return x => x.Key == obj.Key;
        }
    }
}