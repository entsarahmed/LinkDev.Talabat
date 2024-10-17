using LinkDev.Talabat.Core.Domain.Common;
using LinkDev.Talabat.Core.Domain.Contracts;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Infrastructure.Persistence._Data.Generic_Repository
{
    internal static class SpecificationsEvaluator<TEntity, TKey>
        where TEntity : BaseEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecifications<TEntity,TKey> spec)
        {
            var query = inputQuery; //_dbContext.Set<TEntity>()

            if (spec.Criteria is not null) //  P => true && true
                query=query.Where(spec.Criteria);

            //query =_dbContext.Set<Product>().where(P => true && true)

            if(spec.OrderByDesc is not null)    
                query=query.OrderByDescending(spec.OrderByDesc);
            else if (spec.OrderBy is not null)// P => P.Name
                query=query.OrderBy(spec.OrderBy);
            //query = _dbContext.Set<Product>().where(P => true && true).OrderBy(P => P.Name)

            if(spec.IsPaginationEnabled)
                query =query.Skip(spec.Skip).Take(spec.Take);   

            // query = _dbContext.Set<Product>().where(P => true && true).OrderBy(P => P.Name).Skip(0).Take(5)


            //include expressions
            //1. P => P.Brand
            //2. P => P.Category
            // ...

            query = spec.Includes.Aggregate(query, (currentQuery, IncludeExpression) => currentQuery.Include(IncludeExpression));
            // query = _dbContext.Set<Product>().where(P => true && true).OrderBy(P => P.Name).Skip(0).Take(5).Include(P => P.Brand) 
            // query = _dbContext.Set<Product>().where(P => true && true).OrderBy(P => P.Name).Skip(0).Take(5).Include(P => P.Brand).Include(P => P.Category)

            return query;
        }
            }
}
