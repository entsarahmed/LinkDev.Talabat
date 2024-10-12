using LinkDev.Talabat.APIs.Controllers.Base;
using LinkDev.Talabat.Core.Application.Abstraction.Models.Products;
using LinkDev.Talabat.Core.Application.Abstraction.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.APIs.Controllers.Controllers.Products
{
    public class ProductsController(IServiceManager serviceManager) : BaseApiController
    {
        [HttpGet] // GET: /api/products
        public async Task<ActionResult<IEnumerable<ProductToReturnDto>>> GetAllProducts([FromQuery]ProductSpecParams specParams)
        {
            var products = await serviceManager.ProductService.GetProductsAsync(specParams);

            return Ok(products);


        }


        [HttpGet("{id:int}")] //Get: /api/products/id
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var product = await serviceManager.ProductService.GetProductAsync(id);


            if (product is null)
                return NotFound(new { StatusCode = 404, message = "not found" });
                return Ok(product);
        
        }


        [HttpGet("brands")]  //Get: /api/products/brands
        public async Task<ActionResult<IEnumerable<BrandDto>>> GetBrands()
        {
            var brands = await serviceManager.ProductService.GetBrandsAsync();
            return Ok(brands);
        }
        [HttpGet("categories")] //Get: /api/products/categories

        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories()
        {
            var categories = await serviceManager.ProductService.GetCategoriesAsync();
            return Ok(categories);
        }
    }
}
