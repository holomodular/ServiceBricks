using System.Linq.Expressions;

namespace ServiceBricks.Storage.EntityFrameworkCore
{
    /// <summary>
    /// This is the base class that all domain objects inherit from.
    /// </summary>
    /// <typeparam name="TDomainObject"></typeparam>
    public abstract partial class EntityFrameworkCoreDomainObject<TDomainObject> : DomainObject<TDomainObject>, IEntityFrameworkCoreDomainObject<TDomainObject>
    {
        /// <summary>
        /// Provide any default query options.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public virtual IQueryable<TDomainObject> DomainGetIQueryableDefaults(IQueryable<TDomainObject> query)
        {
            return query;
        }

        /// <summary>
        /// Provide an expression that will filter an object based on its primary key.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public abstract Expression<Func<TDomainObject, bool>> DomainGetItemFilter(TDomainObject obj);
    }
}