
using LinkDev.Talabat.APIs.Extensions;
using LinkDev.Talabat.APIs.Service;
using LinkDev.Talabat.Core.Application.Abstraction;
using LinkDev.Talabat.Infrastructure.Persistence;
using LinkDev.Talabat.Infrastructure.Persistence.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace LinkDev.Talabat.APIs
{
    public class Program
    {
       // public static StoreContext StoreContext { get; set; } = null!;
        // Entry Point
        public static async Task Main(string[] args)
        {

            var webApplicationBuilder = WebApplication.CreateBuilder(args);
            
            
            #region Configure Services
            // Add services to the container.

            webApplicationBuilder.Services.AddControllers();// Register Required Service by ASP.NET Core with APIs to DI Container.
                                                            // 


            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            webApplicationBuilder.Services.AddEndpointsApiExplorer().AddSwaggerGen();


            webApplicationBuilder.Services.AddHttpContextAccessor();
            webApplicationBuilder.Services.AddScoped(typeof(ILoggedInUserService),typeof(LoggedInUserService));


            webApplicationBuilder.Services.AddPersistenceServices(webApplicationBuilder.Configuration);
            //DependenecyInjection.AddPersistenceServices(webApplicationBuilder.Services,webApplicationBuilder.Configuration);
            #endregion

            var app = webApplicationBuilder.Build();

            #region implicitly want to clr create object from class DbContext
            ////try
            ////{
            ////    var PendingMigrations = StoreContext.Database.GetPendingMigrations();

            ////    if(PendingMigrations.Any()) 
            ////          await  StoreContext.Database.MigrateAsync(); //Update-Database
            ////}
            ////catch(Exception ex) 
            ////{

            ////}
            ////finally
            ////{
            //// await   StoreContext.DisposeAsync();
            ////} 
            #endregion


            #region Database Initialization

          await  app.InitializerStoreContextAsync();

            #endregion


            #region Cofigure Kestrel Middleware

            // Configure the HTTP request pipeline.

            //check he I exist in which Environment  
            if (app.Environment.IsDevelopment())
            {
                //Generator Documentation special Swagger
                // Internary need Service it that is work dependence injection inside Configure Service 
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

           // app.UseAuthorization();


            app.MapControllers();

            #endregion


            app.Run();
        }
    }
}
