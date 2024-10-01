using System.Text;

namespace ServiceBricks
{
    /// <summary>
    /// This is a response.
    /// </summary>
    public partial class Response : IResponse
    {
        protected List<IResponseMessage> _messages = null;

        /// <summary>
        /// Constructor. By default, all responses are successful unless otherwise set.
        /// </summary>
        public Response()
        {
            Success = true;
            _messages = new List<IResponseMessage>();
        }

        /// <summary>
        /// Determine if the response is successful or if an error happened.
        /// </summary>
        public virtual bool Success { get; set; }

        /// <summary>
        /// Determine if the response is successful or if an error happened.
        /// </summary>
        public virtual bool Error
        { get { return !Success; } set { Success = !value; } }

        /// <summary>
        /// The collection of response messages
        /// </summary>
        public virtual IReadOnlyList<IResponseMessage> Messages
        {
            get
            {
                return _messages.AsReadOnly();
            }
        }

        /// <summary>
        /// Copy the response to this instance.
        /// </summary>
        /// <param name="from"></param>
        public virtual void CopyFrom(IResponse from)
        {
            if (from == null)
                return;

            if (!from.Success)
                this.Success = false;

            if (from.Messages == null || this._messages == null)
                return;

            if (from.Messages.Count > 0)
            {
                foreach (var item in from.Messages)
                {
                    this.AddMessage(item);
                }
            }
        }

        /// <summary>
        /// Copy from this instance to the response object.
        /// </summary>
        /// <param name="to"></param>
        public virtual void CopyTo(IResponse to)
        {
            if (to == null)
                return;

            if (!this.Success)
                to.Success = false;

            if (this._messages == null || to.Messages == null)
                return;

            if (this.Messages.Count > 0)
            {
                foreach (var item in this._messages)
                {
                    to.AddMessage(item);
                }
            }
        }

        /// <summary>
        /// Add a message to the response.
        /// </summary>
        /// <param name="message"></param>
        public virtual void AddMessage(IResponseMessage message)
        {
            if (message == null)
                return;

            if (message.Severity == ResponseSeverity.Error || message.Severity == ResponseSeverity.ErrorSystemSensitive)
                this.Success = false;

            if (_messages != null)
                _messages.Add(message);
        }

        /// <summary>
        /// Add a list of messages to the response.
        /// </summary>
        /// <param name="message"></param>
        public virtual void AddMessage(List<IResponseMessage> messages)
        {
            if (messages == null || messages.Count == 0)
                return;

            foreach (var message in messages)
            {
                if (message.Severity == ResponseSeverity.Error || message.Severity == ResponseSeverity.ErrorSystemSensitive)
                    this.Success = false;

                if (_messages != null)
                    _messages.Add(message);
            }
        }

        /// <summary>
        /// Override ToString() to get list of messages as one string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return System.Text.Json.JsonSerializer.Serialize(this);
        }

        /// <summary>
        /// Get all the messages as a string
        /// </summary>
        /// <param name="seperator"></param>
        /// <returns></returns>
        public virtual string GetMessage(string seperator)
        {
            if (_messages == null && _messages.Count == 0)
            {
                if (Error)
                    return "Error";
                return "Success";
            }

            string outputMessage = string.Empty;
            foreach (var message in _messages)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(Enum.GetName(typeof(ResponseSeverity), message.Severity));
                sb.Append(": ");
                sb.Append(message.Message);
                if (message.Fields != null && message.Fields.Count > 0)
                {
                    sb.Append(" Fields: ");
                    sb.Append(string.Join(",", message.Fields));
                }
                sb.Append(seperator);
                outputMessage += sb.ToString();
            }
            return outputMessage;
        }
    }
}