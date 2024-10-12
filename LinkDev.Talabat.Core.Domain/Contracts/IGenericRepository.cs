﻿using LinkDev.Talabat.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Core.Domain.Contracts
{
    public interface IGenericRepository<TEntity,TKey>
        where TEntity:BaseEntity<TKey> where TKey:IEquatable<TKey>
    {
        Task<IEnumerable<TEntity>> GetAllAsync(bool WithTracking = false);
        Task<IEnumerable<TEntity>> GetAllWithSpecAsync(ISpecifications<TEntity, TKey> spec, bool withTracking = false);

        Task<TEntity?> GetAsync(TKey id);
        Task<TEntity?> GetWithSpecAsync(ISpecifications<TEntity, TKey> spec);

        Task AddAsync(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
    
    }
}
