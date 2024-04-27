using Microsoft.AspNetCore.Builder;

namespace ServiceBricks
{
    /// <summary>
    /// Extensions for the Core Brick.
    /// </summary>
    public static partial class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Start the Core Brick.
        /// </summary>
        /// <param name="applicationBuilder"></param>
        /// <returns></returns>
        public static IApplicationBuilder StartServiceBricks(this IApplicationBuilder applicationBuilder)
        {
            return applicationBuilder;
        }
    }
}