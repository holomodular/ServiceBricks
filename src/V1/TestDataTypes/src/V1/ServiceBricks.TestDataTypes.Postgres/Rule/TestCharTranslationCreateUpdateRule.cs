using ServiceBricks.TestDataTypes.EntityFrameworkCore;

namespace ServiceBricks.TestDataTypes.Postgres
{
    /// <summary>
    /// This is a business rule for creating a Test domain object. It will set the PartitionKey and RowKey.
    /// </summary>
    public sealed class TestCharTranslationCreateUpdateRule : BusinessRule
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public TestCharTranslationCreateUpdateRule()
        {
            Priority = PRIORITY_NORMAL;
        }

        /// <summary>
        /// Register the rule
        /// </summary>
        public static void Register(IBusinessRuleRegistry registry)
        {
            registry.Register(
                typeof(DomainCreateBeforeEvent<Test>),
                typeof(TestCharTranslationCreateUpdateRule));

            registry.Register(
                typeof(DomainUpdateBeforeEvent<Test>),
                typeof(TestCharTranslationCreateUpdateRule));
        }

        /// <summary>
        /// Unregister the rule
        /// </summary>
        /// <param name="registry"></param>
        public static void UnRegister(IBusinessRuleRegistry registry)
        {
            registry.UnRegister(
                typeof(DomainCreateBeforeEvent<Test>),
                typeof(TestCharTranslationCreateUpdateRule));

            registry.UnRegister(
                typeof(DomainUpdateBeforeEvent<Test>),
                typeof(TestCharTranslationCreateUpdateRule));
        }

        /// <summary>
        /// Execute the business rule.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override IResponse ExecuteRule(IBusinessRuleContext context)
        {
            var response = new Response();

            // AI: Make sure the context object is the correct type
            if (context == null || context.Object == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "context"));
                return response;
            }

            Test item = null;
            if (context.Object is DomainCreateBeforeEvent<Test> ec)
            {
                if (ec.DomainObject == null)
                {
                    response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "context"));
                    return response;
                }
                item = ec.DomainObject;
            }
            if (context.Object is DomainUpdateBeforeEvent<Test> eu)
            {
                if (eu.DomainObject == null)
                {
                    response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "context"));
                    return response;
                }
                item = eu.DomainObject;
            }
            if (item != null)
            {
                // AI: Translate Postgres Char values
                char defaultChar = '\0';
                char translateChar = ' ';

            if (item.TestChar == defaultChar)
                    item.TestChar = translateChar;

            if (item.TestCharNull == null || item.TestCharNull == defaultChar)
                    item.TestCharNull = translateChar;

            if (item.TestCharNullWithValue == null || item.TestCharNullWithValue == defaultChar)
                    item.TestCharNullWithValue = translateChar;

            if (item.TestCharNullWithMixedValues == null || item.TestCharNullWithMixedValues == defaultChar)
                    item.TestCharNullWithMixedValues = translateChar;


                return response;
            }

            response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "context"));
            return response;
        }
    }
}
