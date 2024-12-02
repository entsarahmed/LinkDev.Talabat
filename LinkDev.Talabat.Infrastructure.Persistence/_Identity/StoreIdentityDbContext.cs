using LinkDev.Talabat.Core.Domain.Entities.Identity;
using LinkDev.Talabat.Infrastructure.Persistence._Common;
using LinkDev.Talabat.Infrastructure.Persistence._Data;
using LinkDev.Talabat.Infrastructure.Persistence._Identity.Configs;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using System.Text;
using System.Threading.Tasks;



namespace LinkDev.Talabat.Infrastructure.Persistence.Identity
{
    public class StoreIdentityDbContext : IdentityDbContext<ApplicationUser>
    {
        public StoreIdentityDbContext(DbContextOptions<StoreIdentityDbContext> options)
        : base(options) 
        {

            
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(AssemblyInformation).Assembly,
                    type => type.GetCustomAttribute<DbContextTypeAttribute>()?.DbContextType == typeof(StoreIdentityDbContext));
        }

    }
}
