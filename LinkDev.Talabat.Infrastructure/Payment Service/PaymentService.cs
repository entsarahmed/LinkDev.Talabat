using AutoMapper;
using LinkDev.Talabat.Core.Application.Exceptions;
using LinkDev.Talabat.Core.Domain.Contracts.Infrastructure;
using LinkDev.Talabat.Core.Domain.Contracts.Persistence;
using LinkDev.Talabat.Core.Domain.Entities.Basket;
using LinkDev.Talabat.Core.Domain.Entities.Orders;
using LinkDev.Talabat.Shared.Models;
using LinkDev.Talabat.Shared.Models.Basket;
using Microsoft.Extensions.Options;
using Stripe;
using Product = LinkDev.Talabat.Core.Domain.Entities.Products.Product;

namespace LinkDev.Talabat.Infrastructure.Payment_Service
{
    public class PaymentService(
        IBasketRepository basketRepository, 
        IUnitOfWork unitOfWork,IMapper mapper,
        IOptions<RedisSettings> redisSettings,
        IOptions<StripeSettings> stripeSettings) : IPaymentService
    {
        private readonly RedisSettings _redisSettings = redisSettings.Value;
        private readonly StripeSettings _stripeSettings = stripeSettings.Value;

        public async Task<CustomerBasketDto> CreateOrUpdatePaymentIntent(string basketId)
        {

            StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
            var basket = await basketRepository.GetAsync(basketId);
            if (basket is null) throw new NotFoundException(nameof(CustomerBasket),basketId);

            if (basket.DeliveryMethodId.HasValue)
            {
                var deliveryMethod =  await unitOfWork.GetRepository<DeliveryMethod, int>().GetAsync(basket.DeliveryMethodId.Value);
                if (deliveryMethod is null) throw new NotFoundException(nameof(DeliveryMethod), basket.DeliveryMethodId.Value);
                basket.ShippingPrice = deliveryMethod.Cost;
            }


            if (basket.Items.Count() > 0)
            {

                var productRepo =  unitOfWork.GetRepository<Product, int>();
             foreach (var item in basket.Items)
                {
                    var product = await productRepo.GetAsync(item.Id);
                    if (product is null) throw new NotFoundException(nameof(Product), item.Id);
                    if(item.Price != product.Price) 
                        item.Price = product.Price;

                }
            }

            PaymentIntent? paymentIntent = null;
            PaymentIntentService  paymentIntentService = new PaymentIntentService();

            if (string.IsNullOrEmpty(basket.PaymentIntentId)) //Create New Payment Intent
            {
                var options = new PaymentIntentCreateOptions()
                {
                    Amount = (long) basket.Items.Sum(item => item.Price  * 100 * item.Quantity) + (long) basket.ShippingPrice * 100,
                    Currency = "USD",
                    PaymentMethodTypes = new List<string>() {"card"},

                };
             paymentIntent= await paymentIntentService.CreateAsync(options);  //Integration with Stripe
                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;   

            }
            else  // Updated in Existing Payment Intent
            {
                var options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)basket.Items.Sum(item => item.Price  * 100 * item.Quantity) + (long)basket.ShippingPrice * 100,

                };
                await paymentIntentService.UpdateAsync(basket.PaymentIntentId, options);  //Integration with Stripe


            }

            await basketRepository.UpdateAsync(basket, TimeSpan.FromDays(_redisSettings.TimeToLiveInDays));
            return mapper.Map<CustomerBasketDto>(basket);
        }
    }
}
