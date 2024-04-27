namespace ServiceBricks
{
    /// <summary>
    /// This contains common human-readble errors.
    /// </summary>
    public partial class LocalizationResource
    {
        public const string ERROR_LICENSING = @"System error with licensing. You must purchase a license or the software throws exceptions after 30 minutes. Please visit http://ServiceBrick.com to learn more.";

        public const string ERROR_API = "System error with API";
        public const string ERROR_BUSINESS_RULE = "System error with business rules";

        public const string ERROR_BUSINESS_RULE_CONCURRENCY = "System error with concurrency business rule";

        public const string ERROR_BUSINESS_REPOSITORY = "System error with business repository";

        public const string ERROR_BUSINESS_QUEUE_PROCESSOR = "System error with business queue processor";
        public const string ERROR_BUSINESS_QUEUE_PROCESSOR_STOPPED_CRITICAL = "Critical system error queue processor has been stopped by an error item";

        public const string ERROR_PROCESS = "System error with background process";

        public const string ERROR_PROVIDER = "System error with provider";
        public const string ERROR_REST_CLIENT = "System error with rest client";

        public const string ERROR_SECURITY = "System error with security";

        public const string WARNING_BUSINESS_QUEUE_PROCESSOR_STOPPED = "Warning queue processor has been stopped by an error item";

        public const string ERROR_STORAGE = "System Error with storage";

        public const string ERROR_SYSTEM = "System Error";

        public const string ERROR_SERVICEQUERY = "System error with service query";

        public const string ERROR_ITEM_NOT_FOUND = "Item not found";

        public const string ERROR_ITEMS_NOT_FOUND = "Items not found";
        public const string ERROR_USER_NOT_FOUND = "User not found";

        public const string ITEM_DELETED_SUCCESS = "Item deleted successfully";

        public const string ITEM_CREATED_SUCCESS = "Item created successfully";

        public const string ITEM_EDITED_SUCCESS = "Item edited successfully";

        public const string ITEMS_DELETED_SUCCESS = "Items deleted successfully";

        public const string ITEMS_CREATED_SUCCESS = "Items created successfully";

        public const string ITEMS_EDITED_SUCCESS = "Items edited successfully";

        public const string REQUIRED_AT_LEAST_ONE_REQUIRED = "At least one is required";

        public const string REQUIRED_VALUE = "A value is required";

        public const string REQUIRED_FROM_BEFORE_TO = "From must be before To";

        public const string PARAMETER_MISSING = "Parameter is missing";

        public const string UNIT_TEST = "UNIT TEST";
    }
}