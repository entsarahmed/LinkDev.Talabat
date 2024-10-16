using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
            {

            services.AddSingleton(typeof(IServiceCollection), (serviceProvider) =>
            {
                var connectionString = configuration.GetConnectionString("Redis");////GetSection("ConnectionStrings")["Redis"];
                var connectionMultiplexerObj = ConnectionMultiplexer.Connect(connectionString!);
                return connectionMultiplexerObj;
            });
            return services;

            //services.AddScoped(typeof(IConnectionMultiplexer), typeof(ConnectionMultiplexer));            return services;
            }
    }
}
