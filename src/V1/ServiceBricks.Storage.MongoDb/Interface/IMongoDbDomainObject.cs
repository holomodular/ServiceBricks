using System.Linq.Expressions;

namespace ServiceBricks.Storage.MongoDb
{
    public interface IMongoDbDomainObject<TDomain> : IDomainObject<TDomain>
    {
        Expression<Func<TDomain, bool>> DomainGetItemFilter(TDomain obj);
    }
}