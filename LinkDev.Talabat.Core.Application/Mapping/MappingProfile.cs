using AutoMapper;
using Castle.Core.Configuration;
using LinkDev.Talabat.Core.Application.Abstraction.Models.Products;
using LinkDev.Talabat.Core.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Core.Application.Mapping
{
    internal class MappingProfile : Profile
    {
        public MappingProfile( IConfiguration configuration)
        {
            CreateMap<Product, ProductToReturnDto>()
                .ForMember(d => d.Brand, O => O.MapFrom(src => src.Brand!.Name))
                .ForMember(d => d.Category, O => O.MapFrom(src => src.Category!.Name))
                .ForMember(d => d.PictureUrl, O => O.MapFrom(S => $"{"https://localhost:7033}{S.PictureUrl}") );

            CreateMap<ProductBrand, BrandDto>();

            CreateMap<ProductCategory, CategoryDto>();
        }
    }
}
