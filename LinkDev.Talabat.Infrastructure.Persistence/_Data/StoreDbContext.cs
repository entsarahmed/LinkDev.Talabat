using LinkDev.Talabat.Core.Domain.Entities.Employees;
using LinkDev.Talabat.Core.Domain.Entities.Products;
using LinkDev.Talabat.Infrastructure.Persistence._Common;
using LinkDev.Talabat.Infrastructure.Persistence.Identity;
using System.Reflection;

namespace LinkDev.Talabat.Infrastructure.Persistence._Data
{
    public class StoreDbContext : DbContext
    {
        public DbSet<Product> products { get; set; }
        public DbSet<ProductBrand> Brands { get; set; }
        public DbSet<ProductCategory> categories { get; set; }
        // DbSet for Employee entity
      //  public DbSet<Employee> Employees { get; set; }

        // DbSet for Department entity
       // public DbSet<Department> Departments { get; set; }

        public StoreDbContext(DbContextOptions<StoreDbContext> options) : base(options)
        {


        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(AssemblyInformation).Assembly,
                    type => type.GetCustomAttribute<DbContextTypeAttribute>()?.DbContextType == typeof(StoreDbContext));
        }

        //public async Task InitializeAsync()
        //{
        //    // This will apply any pending migrations
        //    if (Database.GetPendingMigrations().Any())
        //    {
        //        await Database.MigrateAsync();
        //    }
        //}

        //public Task SeedAsync()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
