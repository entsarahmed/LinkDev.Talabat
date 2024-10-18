using LinkDev.Talabat.Core.Domain.Contracts;
using LinkDev.Talabat.Infrastructure.Persistence._Data;
using LinkDev.Talabat.Infrastructure.Persistence._Data.Interceptor;
using LinkDev.Talabat.Infrastructure.Persistence.Data;
using LinkDev.Talabat.Infrastructure.Persistence.Identity;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LinkDev.Talabat.Infrastructure.Persistence
{
    public static class DependenecyInjection
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {

            #region Store DbContext
            services.AddDbContext<StoreDbContext>((optionsBuilder) =>
               {

                   optionsBuilder
                   .UseLazyLoadingProxies()
                   .UseSqlServer(configuration.GetConnectionString("StoreContext"));
               }/*, contextLifetime: ServiceLifetime.Scoped, optionsLifetime: ServiceLifetime.Scoped*/); // Select context Life Time, options Life Time

            // services.AddScoped<IStoreContextInitializer, StoreContextInitializer>();
            services.AddScoped(typeof(IStoreContextInitializer), typeof(StoreDbContextInitializer));
            services.AddScoped(typeof(ISaveChangesInterceptor), typeof(CustomSaveChangesInterceptor));


            #endregion

            #region Identity DbContext
            services.AddDbContext<StoreIdentityDbContext>((optionsBuilder) =>
            {

                optionsBuilder
                .UseLazyLoadingProxies()
                .UseSqlServer(configuration.GetConnectionString("IdentityContext"));
            }/*, contextLifetime: ServiceLifetime.Scoped, optionsLifetime: ServiceLifetime.Scoped*/); // Select context Life Time, options Life Time

            #endregion

            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork.UnitOfWork));
            return services;
        }
    }
}
