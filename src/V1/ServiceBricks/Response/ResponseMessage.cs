namespace ServiceBricks
{
    /// <summary>
    /// This is a response message.
    /// </summary>
    public partial class ResponseMessage : IResponseMessage
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public ResponseMessage()
        {
            Fields = new List<string>();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ResponseMessage(IResponseMessage message) : this()
        {
            if (message == null)
                return;

            this.Fields = message.Fields;
            this.Message = message.Message;
            this.Severity = message.Severity;
        }

        /// <summary>
        /// The severity of the message.
        /// </summary>
        public virtual ResponseSeverity Severity { get; set; }

        /// <summary>
        /// The message displayed to the user.
        /// </summary>
        public virtual string Message { get; set; }

        /// <summary>
        /// The field(s) this messages correlates to.
        /// </summary>
        public virtual System.Collections.Generic.IList<string> Fields { get; set; }

        /// <summary>
        /// Get the response message as a string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string message = string.Empty;

            message += Enum.GetName(typeof(ResponseSeverity), Severity) + ": ";

            message += Message;
            if (Fields != null && Fields.Count > 0)
                message += " Fields: " + string.Join(",", Fields);
            return message;
        }

        /// <summary>
        /// Create a success response.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static IResponseMessage CreateSuccess(string message)
        {
            ResponseMessage item = new ResponseMessage
            {
                Message = message,
                Severity = ResponseSeverity.Success,
            };
            return item;
        }

        /// <summary>
        /// Create a success messasge.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public static IResponseMessage CreateSuccess(string message, System.Collections.Generic.IList<string> fields)
        {
            ResponseMessage item = new ResponseMessage();
            if (fields != null)
                item.Fields = fields;
            item.Message = message;
            item.Severity = ResponseSeverity.Success;
            return item;
        }

        /// <summary>
        /// Create a success message.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public static IResponseMessage CreateSuccess(string message, params string[] fields)
        {
            ResponseMessage item = new ResponseMessage();
            if (fields != null && fields.Length > 0)
                item.Fields = new List<string>(fields);
            item.Message = message;
            item.Severity = ResponseSeverity.Success;
            return item;
        }

        /// <summary>
        /// Create an info message.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static IResponseMessage CreateInfo(string message)
        {
            return CreateInfo(message, null);
        }

        /// <summary>
        /// Create an info message.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public static IResponseMessage CreateInfo(string message, System.Collections.Generic.IList<string> fields)
        {
            ResponseMessage item = new ResponseMessage();
            if (fields != null)
                item.Fields = fields;
            item.Message = message;
            item.Severity = ResponseSeverity.Info;
            return item;
        }

        /// <summary>
        /// Create an info message.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public static IResponseMessage CreateInfo(string message, params string[] fields)
        {
            ResponseMessage item = new ResponseMessage();
            if (fields != null && fields.Length > 0)
                item.Fields = new List<string>(fields);
            item.Message = message;
            item.Severity = ResponseSeverity.Info;
            return item;
        }

        /// <summary>
        /// Create a warning message.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static IResponseMessage CreateWarning(string message)
        {
            return CreateWarning(message, null);
        }

        /// <summary>
        /// Create a warning message.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public static IResponseMessage CreateWarning(string message, IList<string> fields)
        {
            ResponseMessage item = new ResponseMessage();
            if (fields != null)
                item.Fields = fields;
            item.Message = message;
            item.Severity = ResponseSeverity.Warning;
            return item;
        }

        /// <summary>
        /// Create a warning message.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public static IResponseMessage CreateWarning(string message, params string[] fields)
        {
            ResponseMessage item = new ResponseMessage();
            if (fields != null && fields.Length > 0)
                item.Fields = new List<string>(fields);
            item.Message = message;
            item.Severity = ResponseSeverity.Warning;
            return item;
        }

        /// <summary>
        /// Create an error message.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static IResponseMessage CreateError(string message)
        {
            return CreateError(message, null);
        }

        /// <summary>
        /// Create an error message.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public static IResponseMessage CreateError(string message, IList<string> fields)
        {
            ResponseMessage item = new ResponseMessage();
            if (fields != null)
                item.Fields = fields;
            item.Message = message;
            item.Severity = ResponseSeverity.Error;
            return item;
        }

        /// <summary>
        /// Create an error message.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public static IResponseMessage CreateError(string message, params string[] fields)
        {
            ResponseMessage item = new ResponseMessage();
            if (fields != null && fields.Length > 0)
                item.Fields = new List<string>(fields);
            item.Message = message;
            item.Severity = ResponseSeverity.Error;
            return item;
        }

        /// <summary>
        /// Create an error message.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public static List<IResponseMessage> CreateError(Exception ex)
        {
            List<IResponseMessage> messages = new List<IResponseMessage>();

            ResponseMessage item = new ResponseMessage();
            item.Message = ex.Message + " " + ex.ToString();
            item.Severity = ResponseSeverity.ErrorSystemSensitive;
            messages.Add(item);

            return messages;
        }

        /// <summary>
        /// Create an error message.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public static List<IResponseMessage> CreateError(Exception ex, string message, IList<string> fields = null)
        {
            List<IResponseMessage> messages = new List<IResponseMessage>();

            ResponseMessage item = new ResponseMessage();
            item.Message = message;
            if (fields != null)
                item.Fields = fields;
            item.Severity = ResponseSeverity.Error;
            messages.Add(item);

            item = new ResponseMessage();
            item.Message = ex.Message + " " + ex.ToString();
            item.Severity = ResponseSeverity.ErrorSystemSensitive;
            messages.Add(item);

            return messages;
        }
    }
}