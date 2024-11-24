using LinkDev.Talabat.Core.Application.Abstraction;
using LinkDev.Talabat.Core.Domain.Common;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Infrastructure.Persistence._Data.Interceptor
{
    internal class AuditInterceptor : SaveChangesInterceptor
    {
        private readonly ILoggedInUserService _loggedInUserService;

        public AuditInterceptor(ILoggedInUserService loggedInUserService)
        {
            _loggedInUserService=loggedInUserService;
        }
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            UpdateEntities(eventData.Context);
            return base.SavingChanges(eventData, result);
        }
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            UpdateEntities(eventData.Context);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }
        

        private void UpdateEntities(DbContext? dbContext)
        {
            if (dbContext is null)
                return;
            var entries = dbContext.ChangeTracker.Entries<IBaseAuditableEntity>()
   .Where(entity => entity.State is EntityState.Added or EntityState.Modified);
            foreach (var entry in entries)
            {
                if (entry.State is EntityState.Added)
                {
                    entry.Entity.CreatedBy=_loggedInUserService.UserId!;
                    entry.Entity.CreatedOn=DateTime.UtcNow;

                }
                entry.Entity.LastModifiedBy=_loggedInUserService.UserId!;
                entry.Entity.LastModifiedOn = DateTime.UtcNow;


            }
            //var entries = dbContext.ChangeTracker.Entries()
            //     .Where(entry => entry.GetType().IsSubclassOf(typeof(BaseAuditableEntity<>)) && entry is { State: EntityState.Added or EntityState.Modified });
            //foreach ( var entry in entries )
            //{
            //    var entity = entry.Entity;
            //    var userId = _loggedInUserService.UserId;
            //    var currentTime = DateTime.UtcNow;
            //    if (entry.State == EntityState.Added )
            //    {
            //        SetPropertyExists(entity, "CreatedBy", userId!);
            //        SetPropertyExists(entity, "CreatedOn", currentTime);

            //    }
            //    SetPropertyExists(entity, "LastModifiedBy", userId!);
            //    SetPropertyExists(entity, "LastModifiedOn", currentTime);


            //}
        }
    
    
    //private void SetPropertyExists(object entity, string propertyName, object value)
    //    {
    //        var property = entity.GetType().GetProperty(propertyName);
    //        if(property != null && property.CanWrite)
    //        {
    //            property.SetValue(entity, value);
    //        }
    //    }
    
    }
}
