namespace ServiceBricks
{
    /// <summary>
    /// This is REST API configurations.
    /// </summary>
    public class ApiConfig
    {
        public string BaseServiceUrl { get; set; }
        public string TokenUrl { get; set; }
        public string TokenType { get; set; }
        public string TokenClient { get; set; }
        public string TokenSecret { get; set; }
        public string TokenResponseType { get; set; }
        public string TokenScope { get; set; }
        public bool ReturnResponseObject { get; set; }
        public bool DisableAuthentication { get; set; }
    }
}