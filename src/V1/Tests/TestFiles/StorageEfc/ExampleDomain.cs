﻿using System.Linq.Expressions;

namespace ServiceBricks.Storage.EntityFrameworkCore.Xunit
{
    public class ExampleDomain : EntityFrameworkCoreDomainObject<ExampleDomain>, IDpCreateDate, IDpUpdateDate
    {
        public int Key { get; set; }
        public string Name { get; set; }

        public DateTimeOffset CreateDate { get; set; }
        public DateTimeOffset UpdateDate { get; set; }
        public DateTimeOffset ExampleDate { get; set; }

        public override Expression<Func<ExampleDomain, bool>> DomainGetItemFilter(ExampleDomain obj)
        {
            return x => x.Key == obj.Key;
        }
    }
}