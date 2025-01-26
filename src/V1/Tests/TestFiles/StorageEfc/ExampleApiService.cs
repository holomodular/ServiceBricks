using AutoMapper;
using ServiceBricks.Xunit;

namespace ServiceBricks.Storage.EntityFrameworkCore.Xunit
{
    public class ExampleApiService : ApiService<ExampleDomain, ExampleDto>, IExampleApiService
    {
        public ExampleApiService(
            IMapper mapper,
            IBusinessRuleService businessRuleService,
            IDomainRepository<ExampleDomain> domainRepository)
            : base(mapper, businessRuleService, domainRepository)
        {
        }
    }

    public class ExampleProcessQueueApiService : ApiService<ExampleWorkProcessDomain, ExampleWorkProcessDto>, IExampleProcessQueueApiService
    {
        public ExampleProcessQueueApiService(
            IMapper mapper,
            IBusinessRuleService businessRuleService,
            IDomainRepository<ExampleWorkProcessDomain> domainRepository)
            : base(mapper, businessRuleService, domainRepository)
        {
        }
    }
}