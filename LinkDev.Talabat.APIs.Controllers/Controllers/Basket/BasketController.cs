using LinkDev.Talabat.APIs.Controllers.Base;
using LinkDev.Talabat.Core.Application.Abstraction.Common.Contracts.Infrastructure;
using LinkDev.Talabat.Shared.Models.Basket;
using Microsoft.AspNetCore.Mvc;

namespace LinkDev.Talabat.APIs.Controllers.Controllers.Basket
{
    public class BasketController(IBasketService basketService) : BaseApiController
    {
        [HttpGet] // GET: /api/Basket?id=
        public async Task<ActionResult<CustomerBasketDto>> GetBasket(string id)
        {
            var basket = await basketService.GetCustomerBasketAsync(id);
            return Ok(basket);
        }
        [HttpPost] //Post: /api/Basket
        public async  Task<ActionResult<CustomerBasketDto>> UpdateBasket(CustomerBasketDto basketDto)
        {
            var basket = await basketService.UpdateCustomerBasketAsync(basketDto);
             
            return Ok(basket);
        }

        [HttpDelete] // Delete: /api/Basket
        public async Task DeleteBasket(string id)
        {
            await basketService.DeleteCustomerBasketAsync(id);
        }
    }
}
