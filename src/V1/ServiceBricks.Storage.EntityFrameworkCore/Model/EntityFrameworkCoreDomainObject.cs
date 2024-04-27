using System.Linq.Expressions;

namespace ServiceBricks.Storage.EntityFrameworkCore
{
    /// <summary>
    /// This is the base class that all domain objects inherit from.
    /// </summary>
    /// <typeparam name="TDomainObject"></typeparam>
    public abstract class EntityFrameworkCoreDomainObject<TDomainObject> : DomainObject<TDomainObject>, IEntityFrameworkCoreDomainObject<TDomainObject>
    {
        public virtual IQueryable<TDomainObject> DomainGetIQueryableDefaults(IQueryable<TDomainObject> query)
        {
            return query;
        }

        public abstract Expression<Func<TDomainObject, bool>> DomainGetItemFilter(TDomainObject obj);
    }
}