using LinkDev.Talabat.Core.Domain.Contracts.Infrastructure;
using LinkDev.Talabat.Infrastructure.Basket_Repository;
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
            services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));
            return services;
            //services.AddScoped(typeof(IConnectionMultiplexer), typeof(ConnectionMultiplexer));            return services;
        }
    }
}
