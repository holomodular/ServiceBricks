using System.Text;

namespace ServiceBricks
{
    /// <summary>
    /// This is the base exception for all managed exceptions in the platform.
    /// Developers should expect that the "Message" property will be
    /// displayed to the user and must not contain any sensitive information.
    /// </summary>
    public partial class BusinessException : Exception, IBusinessException
    {
        protected List<IResponseMessage> _messages = new List<IResponseMessage>();

        /// <summary>
        /// Constructor.
        /// </summary>
        public BusinessException() : base()
        {
            AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_SYSTEM));
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public BusinessException(IResponse resp) : base()
        {
            CopyFrom(resp);
            if (_messages.Count == 0)
                AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_SYSTEM));
            else
                Error = true;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message"></param>
        public BusinessException(string message) : base(message)
        {
            AddMessage(ResponseMessage.CreateError(message));
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="innerException"></param>
        public BusinessException(Exception innerException) : base(LocalizationResource.ERROR_SYSTEM, innerException)
        {
            AddMessage(ResponseMessage.CreateError(innerException, LocalizationResource.ERROR_SYSTEM));
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public BusinessException(Exception innerException, string message) : base(message, innerException)
        {
            if (string.IsNullOrEmpty(message))
                AddMessage(ResponseMessage.CreateError(innerException, LocalizationResource.ERROR_SYSTEM));
            else
                AddMessage(ResponseMessage.CreateError(innerException, message));
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message"></param>
        public BusinessException(IResponseMessage message) : base(message != null ? message.Message : LocalizationResource.ERROR_SYSTEM)
        {
            if (message == null)
                AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_SYSTEM));
            else
            {
                AddMessage(message);
                Error = true;
            }
        }

        /// <summary>
        /// Determine if execution is successful.
        /// </summary>
        public virtual bool Success { get; set; }

        /// <summary>
        /// Determine if execution is successful.
        /// </summary>
        public virtual bool Error
        { get { return !Success; } set { Success = !value; } }

        /// <summary>
        /// List of validation messages.
        /// </summary>
        public virtual IReadOnlyList<IResponseMessage> Messages
        {
            get
            {
                return _messages;
            }
        }

        /// <summary>
        /// Copy a response to this object.
        /// </summary>
        /// <param name="from"></param>
        public virtual void CopyFrom(IResponse from)
        {
            if (from == null)
                return;

            if (!from.Success)
                this.Success = false;

            if (from.Messages == null || this.Messages == null)
                return;

            if (from.Messages.Count > 0)
            {
                foreach (var item in from.Messages)
                {
                    AddMessage(item);
                }
            }
        }

        /// <summary>
        /// Copy this to a response object.
        /// </summary>
        /// <param name="to"></param>
        public virtual void CopyTo(IResponse to)
        {
            if (to == null)
                return;

            if (!this.Success)
                to.Success = false;

            if (this.Messages == null || to.Messages == null)
                return;

            if (this.Messages.Count > 0)
            {
                foreach (var item in this.Messages)
                {
                    to.AddMessage(item);
                }
            }
        }

        /// <summary>
        /// Add message to the response.
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
        /// Get all the messages as a string
        /// </summary>
        /// <param name="seperator"></param>
        /// <returns></returns>
        public virtual string GetMessage(string seperator)
        {
            if (_messages == null || _messages.Count == 0)
            {
                if (Error)
                    return "Error";
                return "Success";
            }

            StringBuilder sb = new StringBuilder();
            foreach (var message in _messages)
            {
                sb.Append(Enum.GetName(typeof(ResponseSeverity), message.Severity));
                sb.Append(": ");
                sb.Append(message.Message);
                if (message.Fields != null && message.Fields.Count > 0)
                {
                    sb.Append(" Fields: ");
                    sb.Append(string.Join(",", message.Fields));
                }
                sb.Append(seperator);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Scrub the response to remove any sensitive system errors.
        /// </summary>
        public virtual void Scrub()
        {
            if (_messages == null)
            {
                if (Error)
                {
                    _messages = new List<IResponseMessage>()
                    {
                        new ResponseMessage()
                        {
                            Severity = ResponseSeverity.Error,
                            Message = LocalizationResource.ERROR_SYSTEM
                        }
                    };
                }
                return;
            }

            for (int i = 0; i < _messages.Count; i++)
            {
                if (_messages[i].Severity == ResponseSeverity.ErrorSystemSensitive)
                {
                    _messages.RemoveAt(i);
                    i--;
                    continue;
                }
            }

            if (Error && !_messages.Any(x => x.Severity == ResponseSeverity.Error))
            {
                _messages.Add(new ResponseMessage()
                {
                    Severity = ResponseSeverity.Error,
                    Message = LocalizationResource.ERROR_SYSTEM
                });
            }
        }
    }
}