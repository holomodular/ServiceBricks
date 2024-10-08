﻿namespace ServiceBricks
{
    /// <summary>
    /// This is a response.
    /// </summary>
    public partial class ResponseCount : Response, IResponseCount
    {
        /// <summary>
        /// Constructor. By default, all responses are successful unless otherwise set.
        /// </summary>
        public ResponseCount() : base() { }

        /// <summary>
        /// The count.
        /// </summary>
        public virtual int? Count { get; set; }
    }
}