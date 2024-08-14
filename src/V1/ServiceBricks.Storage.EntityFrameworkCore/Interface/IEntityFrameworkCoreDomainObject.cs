using System.Linq.Expressions;

namespace ServiceBricks.Storage.EntityFrameworkCore
{
    /// <summary>
    /// The Entity Framework Core domain object.
    /// </summary>
    /// <typeparam name="TDomain"></typeparam>
    public partial interface IEntityFrameworkCoreDomainObject<TDomain> : IDomainObject<TDomain>
    {
        /// <summary>
        /// Provide any default query options.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IQueryable<TDomain> DomainGetIQueryableDefaults(IQueryable<TDomain> query);

        /// <summary>
        /// Provide an expression that will filter an object based on its primary key.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        Expression<Func<TDomain, bool>> DomainGetItemFilter(TDomain obj);
    }
}