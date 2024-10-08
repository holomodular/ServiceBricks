﻿namespace ServiceBricks.Storage.EntityFrameworkCore
{
    /// <summary>
    /// This storage repository uses the Entity Framework Core provider.
    /// </summary>
    /// <typeparam name="TDomain"></typeparam>
    public partial interface IEntityFrameworkCoreStorageRepository<TDomain> : IDbStorageRepository<TDomain>
        where TDomain : class, IEntityFrameworkCoreDomainObject<TDomain>, new()
    {
        /// <summary>
        /// Get a queryable object.
        /// </summary>
        /// <returns></returns>
        IQueryable<TDomain> GetQueryable();
    }
}