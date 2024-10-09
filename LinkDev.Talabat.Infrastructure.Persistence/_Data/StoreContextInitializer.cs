using LinkDev.Talabat.Core.Domain.Contracts;
using LinkDev.Talabat.Core.Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Infrastructure.Persistence.Data
{
    internal class StoreContextInitializer(StoreContext _dbContext) : IStoreContextInitializer
    {
        //private readonly StoreContext _dbContext;

        //public StoreContextInitializer(StoreContext dbContext)
        //{
        //    _dbContext=dbContext;
        //}
        public async Task InitializeAsync()
        {
            
                var PendingMigrations =await _dbContext.Database.GetPendingMigrationsAsync();

                if (PendingMigrations.Any())
                    await _dbContext.Database.MigrateAsync(); //Update-Database
           
            }
        public async Task SeedAsync()
        {
            if (!_dbContext.Brands.Any())
            {
                var brandData = await File.ReadAllTextAsync("../LinkDev.Talabat.Infrastructure.Persistence/_Data/Seeds/brands.json");              

                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandData);
                
                if (brands?.Count > 0)
                {
                    

                    await _dbContext.Set<ProductBrand>().AddRangeAsync(brands);

                    await _dbContext.SaveChangesAsync();

                }


            }

            if (!_dbContext.categories.Any())
            {
                var categoryData = await File.ReadAllTextAsync("F:\\route.net\\course\\API\\Session01\\Demo\\LinkDev.Talabat\\LinkDev.Talabat.Infrastructure.Persistence\\Seeds\\categories.json");   //("../LinkDev.Talabat.Infrastructure.Persistence/Data/Seeds/brands.json");                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandData);

                var categories = JsonSerializer.Deserialize<List<ProductCategory>>(categoryData);

                if (categories?.Count > 0)
                {
                    await _dbContext.Set<ProductCategory>().AddRangeAsync(categories);

                    await _dbContext.SaveChangesAsync();

                }


            }

            if (!_dbContext.products.Any())
            {
                var productData = await File.ReadAllTextAsync("F:\\route.net\\course\\API\\Session01\\Demo\\LinkDev.Talabat\\LinkDev.Talabat.Infrastructure.Persistence\\Seeds\\products.json");   //("../LinkDev.Talabat.Infrastructure.Persistence/Data/Seeds/brands.json");                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandData);

                var products = JsonSerializer.Deserialize<List<Product>>(productData);

                if (products?.Count > 0)
                {


                    await _dbContext.Set<Product>().AddRangeAsync(products);

                    await _dbContext.SaveChangesAsync();

                }


            }
        }
    }
}
