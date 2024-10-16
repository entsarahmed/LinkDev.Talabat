
using LinkDev.Talabat.APIs.Controllers.Errors;
using LinkDev.Talabat.APIs.Extensions;
using LinkDev.Talabat.APIs.Middlewares;
using LinkDev.Talabat.APIs.Service;
using LinkDev.Talabat.Core.Application;
using LinkDev.Talabat.Core.Application.Abstraction;
using LinkDev.Talabat.Infrastructure.Persistence;
using LinkDev.Talabat.Infrastructure.Persistence.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using static System.Net.Mime.MediaTypeNames;

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

            webApplicationBuilder.Services
                .AddControllers()
                .ConfigureApiBehaviorOptions( options =>
                {
                    options.SuppressModelStateInvalidFilter = false;
                    options.InvalidModelStateResponseFactory = (actionContext) =>
                    {
                        var errors = actionContext.ModelState.Where(P => P.Value!.Errors.Count > 0)
                                               .Select(P => new ApiValidationErrorResponse.ValidationError()
                                               {
                                                   Field = P.Key,
                                                   Errors= P.Value!.Errors.Select(E => E.ErrorMessage)
                                               });
                        
                        return new BadRequestObjectResult(new ApiValidationErrorResponse()
                        {

                            Errors = errors
                        });
                    };
                })
                .AddApplicationPart(typeof(Controllers.AssemblyInformation).Assembly);
            // Register Required Service by ASP.NET Core with APIs to DI Container.
            // 
            //webApplicationBuilder.Services.Configure<ApiBehaviorOptions>(options =>
            //{
            //    options.SuppressModelStateInvalidFilter = false;
            //    options.InvalidModelStateResponseFactory = (actionContext) =>
            //    {
            //        var errors = actionContext.ModelState.Where(P => P.Value!.Errors.Count > 0)
            //                               .SelectMany(P => P.Value!.Errors)
            //                               .Select(E => E.ErrorMessage);
            //        return new BadRequestObjectResult(new ApiValidationErrorResponse()
            //        {

            //            Errors = errors
            //        });
            //    };
            //});

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            webApplicationBuilder.Services.AddEndpointsApiExplorer().AddSwaggerGen();


            webApplicationBuilder.Services.AddHttpContextAccessor();
            webApplicationBuilder.Services.AddScoped(typeof(ILoggedInUserService),typeof(LoggedInUserService));


            webApplicationBuilder.Services.AddPersistenceServices(webApplicationBuilder.Configuration);
            //DependenecyInjection.AddPersistenceServices(webApplicationBuilder.Services,webApplicationBuilder.Configuration);

            webApplicationBuilder.Services.AddApplicationServices();
            


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
            app.UseMiddleware<ExceptionHandlerMiddleware>();

            //check he I exist in which Environment  
            if (app.Environment.IsDevelopment())
            {
                //Generator Documentation special Swagger
                // Internary need Service it that is work dependence injection inside Configure Service 
                app.UseSwagger();
                app.UseSwaggerUI();
                //app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseHttpsRedirection();

            app.UseStatusCodePagesWithReExecute("/Errors/{0}");

            app.UseAuthentication();
           app.UseAuthorization();
            


            app.MapControllers();

            #endregion



            app.Run();

        }
    }
}
