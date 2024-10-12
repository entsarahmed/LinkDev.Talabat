using LinkDev.Talabat.Core.Domain.Common;
using LinkDev.Talabat.Core.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Core.Domain.Specifications
{
    public abstract class BaseSpecifications<TEntity, TKey> : ISpecifications<TEntity, TKey>
        where TEntity : BaseEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        public Expression<Func<TEntity,bool>>? Criteria { get; set; } = null;
        public List<Expression<Func<TEntity, object>>> Includes { get; set; } = new ();
        public Expression<Func<TEntity, object>>? OrderBy { get; set; } = null;
        public Expression<Func<TEntity, object>>? OrderByDesc { get; set; } = null;

        public BaseSpecifications()
        {
            //Criteria = null;

        }

        public BaseSpecifications(TKey id)
        {
            Criteria = E => E.Id.Equals(id);
        }

        private protected virtual void AddInclude()
        {
           
        }

        private protected virtual void AddOrderBy(Expression<Func<TEntity, object>> orderByExpression)
        {
            OrderBy = orderByExpression;  // P => P.Name
        }

        private protected virtual void AddOrderByDesc(Expression<Func<TEntity, object>> orderByExpressionDesc)
        {
            OrderByDesc = orderByExpressionDesc; // P => P.Price
        }

    }
}
