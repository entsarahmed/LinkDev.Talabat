using LinkDev.Talabat.Core.Domain.Common;
using LinkDev.Talabat.Core.Domain.Contracts;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Infrastructure.Persistence._Data.Repositories.Generic_Repository
{
    internal static class SpecificationsEvaluator<TEntity, TKey>
        where TEntity : BaseEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecifications<TEntity,TKey> spec)
        {
            var query = inputQuery; //_dbContext.Set<TEntity>()

            if (spec.Criteria is not null) // P => P.Id.Equals(1)
                query=query.Where(spec.Criteria);

            //query =_dbContext.Set<Product>().where(P => P.Id.Equals(1))
            //include expressions
            //1. P => P.Brand
            //2. P => P.Category
            // ...

            query = spec.Includes.Aggregate(query, (currentQuery, IncludeExpression) => currentQuery.Include(IncludeExpression));

            //query =_dbContext.Set<Product>().where(P => P.Id.Equals(1)).Include(P => P.Brand)

            //            //query =_dbContext.Set<Product>().where(P => P.Id.Equals(1)).Include(P => P.Brand).Include(P => P.Category)

            return query;
        }
            }
}
