using AutoMapper;
using Castle.Core.Logging;
using LinkDev.Talabat.Core.Application.Abstraction.Common.Contracts.Infrastructure;
using LinkDev.Talabat.Core.Application.Exceptions;
using LinkDev.Talabat.Core.Domain.Contracts.Infrastructure;
using LinkDev.Talabat.Core.Domain.Contracts.Persistence;
using LinkDev.Talabat.Core.Domain.Entities.Basket;
using LinkDev.Talabat.Core.Domain.Entities.Orders;
using LinkDev.Talabat.Core.Domain.Specifications.Orders;
using LinkDev.Talabat.Shared.Models;
using LinkDev.Talabat.Shared.Models.Basket;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Stripe;
using Product = LinkDev.Talabat.Core.Domain.Entities.Products.Product;

namespace LinkDev.Talabat.Infrastructure.Payment_Service
{
    public class PaymentService(
        IBasketRepository basketRepository, 
        IUnitOfWork unitOfWork,IMapper mapper,
        IOptions<RedisSettings> redisSettings,
        IOptions<StripeSettings> stripeSettings,
        ILogger<PaymentService> logger
        ) : IPaymentService
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

        public async Task UpdateOrderPaymentStatus(string requestBody, string header)
        
            {
                var stripeEvent = EventUtility.ConstructEvent(requestBody,header, _stripeSettings.WebhookSecret);

            //Handle the event 
            var paymentIntent = (PaymentIntent)stripeEvent.Data.Object;

            Order? order;

                switch (stripeEvent.Type)
                {
                    case "payment_intent.succeeded":
                        order =  await UpdatePaymentIntent(paymentIntent.Id, isPaid: true);
                       logger.LogInformation("ORDER is Succeeded with Payment IntentId: {0}", paymentIntent.Id);
                       break;
                    case "payment_intent.payment_failed":
                        order = await UpdatePaymentIntent(paymentIntent.Id, isPaid: false);
                       logger.LogInformation("ORDER is !Succeeded with Payment IntentId: {0}", paymentIntent.Id);
                       break;

                }
        }
        private async Task<Order> UpdatePaymentIntent(string paymentIntentId, bool isPaid)
        {
            var orderRepo = unitOfWork.GetRepository<Order, int>();

            var spec = new OrderByPaymentIntentSpecifications(paymentIntentId);

            var order = await orderRepo.GetWithSpecAsync(spec);

            if (order is null) throw new NotFoundException(nameof(Order), $"PaymentIntentId: {paymentIntentId}");

            if(isPaid)
                order.Status = OrderStatus.PaymentReceived;
            else
                order.Status = OrderStatus.PaymentFailed;


            orderRepo.Update(order);

            await unitOfWork.CompleteAsync();

            return order;

        }


      
    }
}
