﻿using LinkDev.Talabat.Core.Domain.Common;
using LinkDev.Talabat.Core.Domain.Contracts;
using LinkDev.Talabat.Core.Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Infrastructure.Persistence.Data.Repositories
{
    public class GenericRepositories<TEntity, TKey>(StoreContext _dbContext) : IGenericRepository<TEntity, TKey>
        where TEntity : BaseEntity<TKey> where TKey : IEquatable<TKey>
    {
        //private readonly StoreContext _storeContext;

        //public GenericRepositories(StoreContext storeContext)
        //{
        //    _storeContext=storeContext;
        //}
        public async Task<IEnumerable<TEntity>> GetAllAsync(bool WithTracking = false)
        {

            if (typeof(TEntity) == typeof(Product))
                return WithTracking?
                    
               (IEnumerable<TEntity>) await _dbContext.Set<Product>().Include(P => P.Brand).Include(P => P.Category).ToListAsync() :
               (IEnumerable<TEntity>) await _dbContext.Set<Product>().Include(P => P.Brand).Include(P => P.Category).AsNoTracking().ToListAsync();


            return WithTracking ? await _dbContext.Set<TEntity>().ToListAsync() :
            await _dbContext.Set<TEntity>().AsNoTrackingWithIdentityResolution().ToListAsync();
    
        }
      
    //{
    //    if (WithNoTracking)
    //        return await _dbContext.Set<TEntity>().ToListAsync();
    //    return await _dbContext
    //        .Set<TEntity>().AsNoTracking().ToListAsync();
    //}

    public async Task<TEntity?> GetAsync(TKey id) => await _dbContext.Set<TEntity>().FindAsync(id);

        public async Task AddAsync(TEntity entity) => await _dbContext.Set<TEntity>().AddAsync(entity);



        public void Update(TEntity entity) => _dbContext.Set<TEntity>().Update(entity);
        

        public void Delete(TEntity entity) => _dbContext.Set<TEntity>().Remove(entity);
        


    }
}
