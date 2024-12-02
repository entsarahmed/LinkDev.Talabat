using LinkDev.Talabat.Core.Domain.Contracts.Persistence.DbInitializers;
using LinkDev.Talabat.Core.Domain.Entities.Orders;
using LinkDev.Talabat.Core.Domain.Entities.Products;
using LinkDev.Talabat.Infrastructure.Persistence._Common;
using System.Text.Json;

namespace LinkDev.Talabat.Infrastructure.Persistence._Data
{
   internal sealed class StoreDbInitializer(StoreDbContext _dbContext) : DbIntializer(_dbContext),IStoreDbInitializer
    {
        public override async Task SeedAsync()
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
                var categoryData = await File.ReadAllTextAsync("../LinkDev.Talabat.Infrastructure.Persistence/_Data/Seeds/categories.json");//("F:\\route.net\\course\\API\\Session01\\Demo\\LinkDev.Talabat\\LinkDev.Talabat.Infrastructure.Persistence\\Seeds\\categories.json");   //("../LinkDev.Talabat.Infrastructure.Persistence/Data/Seeds/brands.json");                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandData);

                var categories = JsonSerializer.Deserialize<List<ProductCategory>>(categoryData);

                if (categories?.Count > 0)
                {
                    await _dbContext.Set<ProductCategory>().AddRangeAsync(categories);

                    await _dbContext.SaveChangesAsync();

                }


            }

            if (!_dbContext.products.Any())
            {
                var productData = await File.ReadAllTextAsync("../LinkDev.Talabat.Infrastructure.Persistence/_Data/Seeds/products.json");//("F:\\route.net\\course\\API\\Session01\\Demo\\LinkDev.Talabat\\LinkDev.Talabat.Infrastructure.Persistence\\Seeds\\products.json");   //("../LinkDev.Talabat.Infrastructure.Persistence/Data/Seeds/brands.json");                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandData);

                var products = JsonSerializer.Deserialize<List<Product>>(productData);

                if (products?.Count > 0)
                {


                    await _dbContext.Set<Product>().AddRangeAsync(products);

                    await _dbContext.SaveChangesAsync();

                }


            }

            if (!_dbContext.DeliveryMethods.Any())
            {
                var deliveryMethodData = await File.ReadAllTextAsync("../LinkDev.Talabat.Infrastructure.Persistence/_Data/Seeds/delivery.json");//("F:\\route.net\\course\\API\\Session01\\Demo\\LinkDev.Talabat\\LinkDev.Talabat.Infrastructure.Persistence\\Seeds\\products.json");   //("../LinkDev.Talabat.Infrastructure.Persistence/Data/Seeds/brands.json");                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandData);

                var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryMethodData);

                if (deliveryMethods?.Count > 0)
                {


                    await _dbContext.Set<DeliveryMethod>().AddRangeAsync(deliveryMethods);

                    await _dbContext.SaveChangesAsync();

                }


            }



        }
    }
}
