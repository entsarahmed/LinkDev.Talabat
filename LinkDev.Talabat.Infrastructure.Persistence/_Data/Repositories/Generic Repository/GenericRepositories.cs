using LinkDev.Talabat.Core.Domain.Common;
using LinkDev.Talabat.Core.Domain.Contracts;
using LinkDev.Talabat.Core.Domain.Entities.Products;
using LinkDev.Talabat.Infrastructure.Persistence._Data.Repositories.Generic_Repository;
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

        

            return WithTracking ?
            await _dbContext.Set<TEntity>().ToListAsync() :
            await _dbContext.Set<TEntity>().AsNoTracking().ToListAsync();
    
        }
      
  
    public async Task<TEntity?> GetAsync(TKey id)
        {
    
            return await _dbContext.Set<TEntity>().FindAsync(id);
        }


        public async Task<IEnumerable<TEntity>> GetAllWithSpecAsync(ISpecifications<TEntity, TKey> spec, bool withTracking = false)
        {
            return await ApplySpecifications(spec).ToListAsync();

        }




        public async Task<TEntity?> GetWithSpecAsync(ISpecifications<TEntity, TKey> spec)
        {
            return await ApplySpecifications(spec).FirstOrDefaultAsync();
        }


        public async Task<int> GetCountAsync(ISpecifications<TEntity, TKey> spec)
        {
            return await ApplySpecifications(spec).CountAsync();        }
        public async Task AddAsync(TEntity entity) => await _dbContext.Set<TEntity>().AddAsync(entity);



        public void Update(TEntity entity) => _dbContext.Set<TEntity>().Update(entity);
        

        public void Delete(TEntity entity) => _dbContext.Set<TEntity>().Remove(entity);

        #region Helpers
        private IQueryable<TEntity> ApplySpecifications(ISpecifications<TEntity, TKey> spec)
        {
            return SpecificationsEvaluator<TEntity, TKey>.GetQuery(_dbContext.Set<TEntity>(), spec);
        }

        
        #endregion


    }
}
