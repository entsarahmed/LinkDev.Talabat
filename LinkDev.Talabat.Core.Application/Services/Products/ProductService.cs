﻿using AutoMapper;
using LinkDev.Talabat.Core.Application.Abstraction.Models.Products;
using LinkDev.Talabat.Core.Application.Abstraction.Services.Products;
using LinkDev.Talabat.Core.Domain.Contracts;
using LinkDev.Talabat.Core.Domain.Entities.Products;
using LinkDev.Talabat.Core.Domain.Specifications;
using LinkDev.Talabat.Core.Domain.Specifications.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Core.Application.Services.Products
{
    public class ProductService(IUnitOfWork unitOfWork, IMapper mapper) : IProductService
    {

        public async Task<IEnumerable<ProductToReturnDto>> GetProductsAsync(string? sort)
        {

            var spec = new ProductWithBrandAndCategorySpecifications(sort);
            

            var products = await unitOfWork.GetRepository<Product, int>().GetAllWithSpecAsync(spec);
            var productsToReturn = mapper.Map<IEnumerable<ProductToReturnDto>>(products);

            return productsToReturn;
        }

        public async Task<ProductToReturnDto> GetProductAsync(int id)
        {
            var spec = new ProductWithBrandAndCategorySpecifications(id);

            var product = await unitOfWork.GetRepository<Product, int>().GetWithSpecAsync(spec);
            var productToReturn = mapper.Map<ProductToReturnDto>(product);
            return productToReturn;
        }
        public async Task<IEnumerable<BrandDto>> GetBrandsAsync()
        {
            var brands = await unitOfWork.GetRepository<ProductBrand, int>().GetAllAsync();
            var brandsToReturn = mapper.Map<IEnumerable<BrandDto>>(brands);
            return brandsToReturn;
        }

        public async Task<IEnumerable<CategoryDto>> GetCategoriesAsync()
        {
            var categories = await unitOfWork.GetRepository<ProductCategory, int>().GetAllAsync();

            var categoriesToReturn = mapper.Map<IEnumerable<CategoryDto>>(categories);

            return categoriesToReturn;

        }




    }
}
