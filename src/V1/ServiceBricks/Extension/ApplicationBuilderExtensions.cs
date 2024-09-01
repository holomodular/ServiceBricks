using Microsoft.AspNetCore.Builder;

namespace ServiceBricks
{
    /// <summary>
    /// Extensions for starting ServiceBricks.
    /// </summary>
    public static partial class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Start ServiceBricks
        /// </summary>
        /// <param name="applicationBuilder"></param>
        /// <returns></returns>
        public static IApplicationBuilder StartServiceBricks(this IApplicationBuilder applicationBuilder)
        {
            return applicationBuilder;
        }
    }
}