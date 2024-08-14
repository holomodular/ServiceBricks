using System.Linq.Expressions;

namespace ServiceBricks.Storage.MongoDb
{
    /// <summary>
    /// This is the base class that all domain objects inherit from.
    /// </summary>
    /// <typeparam name="TDomainObject"></typeparam>
    public abstract partial class MongoDbDomainObject<TDomainObject> : DomainObject<TDomainObject>, IMongoDbDomainObject<TDomainObject>
    {
        /// <summary>
        /// Provide an expression that will filter an object based on its primary key.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public abstract Expression<Func<TDomainObject, bool>> DomainGetItemFilter(TDomainObject obj);
    }
}