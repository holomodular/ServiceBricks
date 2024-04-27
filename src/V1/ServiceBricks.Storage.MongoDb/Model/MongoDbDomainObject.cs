using System.Linq.Expressions;

namespace ServiceBricks.Storage.MongoDb
{
    /// <summary>
    /// This is the base class that all domain objects inherit from.
    /// </summary>
    /// <typeparam name="TDomainObject"></typeparam>
    public abstract class MongoDbDomainObject<TDomainObject> : DomainObject<TDomainObject>, IMongoDbDomainObject<TDomainObject>
    {
        public abstract Expression<Func<TDomainObject, bool>> DomainGetItemFilter(TDomainObject obj);
    }
}