using System.Linq.Expressions;

namespace ServiceBricks.Storage.MongoDb
{
    /// <summary>
    /// A domain object that is stored in a MongoDb collection.
    /// </summary>
    /// <typeparam name="TDomain"></typeparam>
    public partial interface IMongoDbDomainObject<TDomain> : IDomainObject<TDomain>
    {
        /// <summary>
        /// Provide an expression that will filter an object based on its primary key.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        Expression<Func<TDomain, bool>> DomainGetItemFilter(TDomain obj);
    }
}