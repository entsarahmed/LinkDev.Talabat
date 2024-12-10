using LinkDev.Talabat.Core.Domain.Contracts.Infrastructure;
using LinkDev.Talabat.Core.Domain.Entities.Basket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Infrastructure.Payment_Service
{
    public class PaymentService : IPaymentService
    {
        public Task<CustomerBasket?> CreateOrUpdatePaymentIntent(string basketId)
        {
            throw new NotImplementedException();
        }
    }
}
