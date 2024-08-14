namespace ServiceBricks
{
    /// <summary>
    /// These are constants for the ServiceBricks modeule.
    /// </summary>
    public partial class ServiceBricksConstants
    {
        /// <summary>
        /// Appsetting for the client api options.
        /// </summary>
        public const string APPSETTING_CLIENT_APIOPTIONS = @"ServiceBricks:Client:Api";

        /// <summary>
        /// Appsetting for the API options.
        /// </summary>
        public const string APPSETTING_APIOPTIONS = @"ServiceBricks:Api";

        /// <summary>
        /// Appsetting for the application options.
        /// </summary>
        public const string APPSETTING_APPLICATIONOPTIONS = @"ServiceBricks:Application";

        /// <summary>
        /// Security policy for the admin.
        /// </summary>
        public const string SECURITY_POLICY_ADMIN = @"ServiceBricksPolicyAdmin";

        /// <summary>
        /// Security policy for the user.
        /// </summary>
        public const string SECURITY_POLICY_USER = @"ServiceBricksPolicyUser";
    }
}