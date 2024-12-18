using LinkDev.Talabat.APIs.Controllers.Base;
using LinkDev.Talabat.Core.Application.Abstraction.Common.Contracts.Infrastructure;
using LinkDev.Talabat.Core.Domain.Contracts.Infrastructure;
using LinkDev.Talabat.Shared.Models.Basket;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using System.Diagnostics.Eventing.Reader;

namespace LinkDev.Talabat.APIs.Controllers.Controllers.Payment
{
    [Authorize]
    public class PaymentController(IPaymentService paymentService) : BaseApiController
    {
        
        [HttpPost("{basketId}")]   //POST: /api/payment/{basketId}
        public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdatePaymentIntent(string basketId)
         {
         var result = await paymentService.CreateOrUpdatePaymentIntent(basketId);
            return Ok(result);
        }

        [HttpPost("api/payment/webhook")]
        public async Task<IActionResult> WebHook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
           
            await paymentService.UpdateOrderPaymentStatus(json, Request.Headers["Stripe-Signature"]!);

            return Ok();
            }
    }

}
