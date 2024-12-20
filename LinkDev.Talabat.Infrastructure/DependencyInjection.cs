using LinkDev.Talabat.Core.Application.Abstraction.Common.Contracts.Infrastructure;
using LinkDev.Talabat.Core.Domain.Contracts.Infrastructure;
using LinkDev.Talabat.Infrastructure.Basket_Repository;
using LinkDev.Talabat.Infrastructure.Cache__Service;
using LinkDev.Talabat.Infrastructure.Payment_Service;
using LinkDev.Talabat.Shared.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace LinkDev.Talabat.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
            {

            services.AddSingleton(typeof(IConnectionMultiplexer), (serviceProvider) =>
            {
                var connectionString = configuration.GetConnectionString("Redis");////GetSection("ConnectionStrings")["Redis"];
                var connectionMultiplexerObj = ConnectionMultiplexer.Connect(connectionString!);
                return connectionMultiplexerObj;
            });
            //services.AddScoped(typeof(IConnectionMultiplexer), (_) =>
            //{
            //    var connectionString = configuration.GetConnectionString("Redis");
            //    var connectionMultiplexerObj = ConnectionMultiplexer.Connect(connectionString!);
            //    return connectionMultiplexerObj;
            //});

            services.AddSingleton(typeof(IResponseCacheService), typeof(ResponseCacheService));

            services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));
            services.AddScoped(typeof(IPaymentService), typeof(PaymentService));

            


            services.Configure<RedisSettings>(configuration.GetSection("RedisSettings"));
            services.Configure<StripeSettings>(configuration.GetSection("StripeSettings"));



            return services;
            //services.AddScoped(typeof(IConnectionMultiplexer), typeof(ConnectionMultiplexer));            return services;
        }
    }
}
