﻿using AutoMapper;
using LinkDev.Talabat.Core.Application.Abstraction.Services;
using LinkDev.Talabat.Core.Application.Abstraction.Services.Basket;
using LinkDev.Talabat.Core.Application.Abstraction.Services.Orders;
using LinkDev.Talabat.Core.Application.Abstraction.Services.Products;
using LinkDev.Talabat.Core.Application.Mapping;
using LinkDev.Talabat.Core.Application.Services;
using LinkDev.Talabat.Core.Application.Services.Basket;
using LinkDev.Talabat.Core.Application.Services.Orders;
using LinkDev.Talabat.Core.Application.Services.Products;
using LinkDev.Talabat.Core.Domain.Contracts.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Core.Application
{
    public static class DependecyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
           // services.AddAutoMapper(Mapper => Mapper.AddProfile(new MappingProfile()));
            services.AddAutoMapper(typeof(MappingProfile));


            services.AddScoped(typeof(IBasketService), typeof(BasketService));
            //services.AddScoped(typeof(Func<IBasketService>), typeof(Func<BasketService>));
            
            services.AddScoped(typeof(Func<IBasketService>), (serviceProvider) =>
            {
                var mapper = serviceProvider.GetRequiredService<IMapper>();
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                var basketRepository = serviceProvider.GetRequiredService<IBasketRepository>();

                return () => new BasketService(basketRepository,mapper,configuration);

                //return () => serviceProvider.GetService<IBasketService>();
            });


            services.AddScoped(typeof(IOrderService), typeof(OrderService));

            //Register the factory for Fun<IOrderService>
            services.AddScoped(typeof(Func<IOrderService>), (serviceProvider) =>
            {
                return () => serviceProvider.GetRequiredService<IOrderService>();   
            });

            services.AddScoped(typeof(IServiceManager), typeof(ServiceManager));

            return services;
        }
    }
}
