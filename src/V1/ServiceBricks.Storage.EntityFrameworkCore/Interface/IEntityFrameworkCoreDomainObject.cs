using System.Linq.Expressions;

namespace ServiceBricks.Storage.EntityFrameworkCore
{
    public interface IEntityFrameworkCoreDomainObject<TDomain> : IDomainObject<TDomain>
    {
        IQueryable<TDomain> DomainGetIQueryableDefaults(IQueryable<TDomain> query);

        Expression<Func<TDomain, bool>> DomainGetItemFilter(TDomain obj);
    }
}