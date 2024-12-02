using LinkDev.Talabat.Core.Domain.Contracts.Persistence.DbInitializers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Infrastructure.Persistence._Common
{
    internal abstract class DbIntializer(DbContext _dbContext) : IDbInitializer
    {
        public virtual async Task InitializeAsync()
        {
            var PendingMigrations = await _dbContext.Database.GetPendingMigrationsAsync();

            if (PendingMigrations.Any())
                await _dbContext.Database.MigrateAsync(); //Update-Database


        }

        public abstract Task SeedAsync();
        
    }
}
