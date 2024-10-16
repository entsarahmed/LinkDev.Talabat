using AutoMapper;
using LinkDev.Talabat.Core.Application.Abstraction.Models.Basket;
using LinkDev.Talabat.Core.Application.Abstraction.Services.Basket;
using LinkDev.Talabat.Core.Application.Exceptions;
using LinkDev.Talabat.Core.Domain.Contracts.Infrastructure;
using LinkDev.Talabat.Core.Domain.Entities.Basket;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Core.Application.Services.Basket
{
    internal class BasketService(IBasketRepository basketRepository,IMapper mapper,IConfiguration configuration ) : IBasketService
    {
        public async Task<CustomerBasketDto> GetCustomerBasketAsync(string basketId)
        {
            var basket = await basketRepository.GetAsync(basketId);
            if(basket is null) throw new NotFoundException(nameof(CustomerBasket), basketId);
            return mapper.Map<CustomerBasketDto>(basket);
        }

        public async Task<CustomerBasketDto> UpdateCustomerBasketAsync(CustomerBasketDto basketDto)
        {
            var basket = mapper.Map<CustomerBasket>(basketDto);
            var timeToLive = TimeSpan.FromDays(double.Parse(configuration.GetSection("RedisSettings")["TimeToLiveInDays"]!));
            var UpdatedBasket = await basketRepository.UpdateAsync(basket, timeToLive);
            if (UpdatedBasket is null) throw new BadRequestException("can't update, there is a problem with this basket");
             return basketDto;
            
        }
        public async Task DeleteCustomerBasketAsync(string basketId)
        {
          var deleted= await basketRepository.DeleteAsync(basketId);
            if (!deleted) throw new BadRequestException("unable to delete this basket.");
        }


    }
}
