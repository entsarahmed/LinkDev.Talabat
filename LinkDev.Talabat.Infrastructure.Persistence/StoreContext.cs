using LinkDev.Talabat.Core.Domain.Common;
using LinkDev.Talabat.Core.Domain.Entities.Employees;
using LinkDev.Talabat.Core.Domain.Entities.Products;
using System.Reflection;

namespace LinkDev.Talabat.Infrastructure.Persistence
{
    public class StoreContext: DbContext
    {
        public DbSet<Product> products { get; set; }
        public DbSet<ProductBrand> Brands { get; set; }
        public DbSet<ProductCategory> categories { get; set; }
        // DbSet for Employee entity
        public DbSet<Employee> Employees { get; set; }

        // DbSet for Department entity
        public DbSet<Department> Departments { get; set; }

        public StoreContext(DbContextOptions<StoreContext> options) : base(options) 
        { 
        
        
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        // modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(StoreContext).Assembly);

        
        }

        public async Task InitializeAsync()
        {
            // This will apply any pending migrations
            if (Database.GetPendingMigrations().Any())
            {
                await Database.MigrateAsync();
            }
        }

        public   async Task SeedAsync()
        {
             throw new NotImplementedException();
        }
    }
}
