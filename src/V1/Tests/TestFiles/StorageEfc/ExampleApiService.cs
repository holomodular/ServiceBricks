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
}