using LinkDev.Talabat.Infrastructure.Persistence;

namespace LinkDev.Talabat.APIs.Extensions
{
    public static class InitializerExtensions
    {
        public static async Task<WebApplication> InitializerStoreContextAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateAsyncScope();
            var services = scope.ServiceProvider;

            var StoreContextInitializer = services.GetRequiredService<StoreContext>();
            //Ask Runtime Env for an Object from "StoreContext" Service Explicitly.

            var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            //var logger = services.GetRequiredService<ILogger<Program>>();

            try
            {
                await StoreContextInitializer.InitializeAsync();
                await StoreContextInitializer.SeedAsync();
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
