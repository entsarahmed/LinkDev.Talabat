using LinkDev.Talabat.Core.Domain.Contracts.Persistence.DbInitializers;

namespace LinkDev.Talabat.APIs.Extensions
{
    public static class InitializerExtensions
    {
        public static async Task<WebApplication> InitializerDbAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateAsyncScope();
            var services = scope.ServiceProvider;

            var storeContextInitializer = services.GetRequiredService<IStoreDbInitializer>();
            var IdentityContextInitializer = services.GetRequiredService<IStoreIdentityDbInitializer>();
            //Ask Runtime Env for an Object from "StoreContext" Service Explicitly.


            var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            //var logger = services.GetRequiredService<ILogger<Program>>();

            try
            {
                await storeContextInitializer.InitializeAsync();
                await storeContextInitializer.SeedAsync();
               
                await IdentityContextInitializer.InitializeAsync();
                await IdentityContextInitializer.SeedAsync();


            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "an error has been occured during applying the migrations or the data Seeding");
            }
            return app;
        }




    }
}
