using LinkDev.Talabat.Core.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Infrastructure.Persistence.Data
{
    public static class StoreContextSeed
    {
        public static async Task SeedAsync (StoreContext dbContext)
        {

            if(!dbContext.Brands.Any())
            {
                var brandData = await File.ReadAllTextAsync("F:\\route.net\\course\\API\\Session01\\Demo\\LinkDev.Talabat\\LinkDev.Talabat.Infrastructure.Persistence\\Seeds\\brands.json");   //("../LinkDev.Talabat.Infrastructure.Persistence/Data/Seeds/brands.json");                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandData);

                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandData);
                //if (brands is not null && brands.Count > 0)
                //{
                //    foreach (var brand in brands)
                //    {
                //        dbContext.Brands.Add(brand);
                //}
                //        await dbContext.SaveChangesAsync();
                
                //}
                if (brands?.Count > 0)
                {
                    //foreach (var brand in brands)
                    //{
                    // await   dbContext.Brands.AddAsync(brand);
                    //}

                    await dbContext.Set<ProductBrand>().AddRangeAsync(brands);

                    await dbContext.SaveChangesAsync();

                }


            }

            if (!dbContext.categories.Any())
            {
                var categoryData = await File.ReadAllTextAsync("F:\\route.net\\course\\API\\Session01\\Demo\\LinkDev.Talabat\\LinkDev.Talabat.Infrastructure.Persistence\\Seeds\\categories.json");   //("../LinkDev.Talabat.Infrastructure.Persistence/Data/Seeds/brands.json");                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandData);

                var categories = JsonSerializer.Deserialize<List<ProductCategory>>(categoryData);
                
                if (categories?.Count > 0)
                {
                    await dbContext.Set<ProductCategory>().AddRangeAsync(categories);

                    await dbContext.SaveChangesAsync();

                }


            }

            if (!dbContext.products.Any())
            {
                var productData = await File.ReadAllTextAsync("F:\\route.net\\course\\API\\Session01\\Demo\\LinkDev.Talabat\\LinkDev.Talabat.Infrastructure.Persistence\\Seeds\\products.json");   //("../LinkDev.Talabat.Infrastructure.Persistence/Data/Seeds/brands.json");                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandData);

                var products = JsonSerializer.Deserialize<List<Product>>(productData);
                
                if (products?.Count > 0)
                {
                   

                    await dbContext.Set<Product>().AddRangeAsync(products);

                    await dbContext.SaveChangesAsync();

                }


            }


        }
    }
}
