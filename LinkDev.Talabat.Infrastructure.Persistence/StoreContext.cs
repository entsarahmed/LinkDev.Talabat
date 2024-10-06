using LinkDev.Talabat.Core.Domain.Entities.Products;
using System.Reflection;

namespace LinkDev.Talabat.Infrastructure.Persistence
{
    public class StoreContext: DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> options) : base(options) 
        { 
        
        
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
         modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(StoreContext).Assembly);

        
        }


        public DbSet<Product> products { get; set; }
        public DbSet<ProductBrand> Brands { get; set; }
        public DbSet<ProductCategory> categories { get; set; }
    }
}
