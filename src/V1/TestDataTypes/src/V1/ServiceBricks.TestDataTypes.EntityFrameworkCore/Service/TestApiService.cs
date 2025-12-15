namespace ServiceBricks.TestDataTypes.EntityFrameworkCore
{
    /// <summary>
    /// This is a REST API service for TestDto.
    /// </summary>
    public partial class TestApiService : ApiService<Test, TestDto>, ITestApiService
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="businessRuleService"></param>
        /// <param name="repository"></param>
        public TestApiService(
            IMapper mapper,
            IBusinessRuleService businessRuleService,
            IDomainRepository<Test> repository)
            : base(mapper, businessRuleService, repository)
        {
        }
    }
}
