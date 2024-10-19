
using LinkDev.Talabat.APIs.Controllers.Errors;
using LinkDev.Talabat.APIs.Extensions;
using LinkDev.Talabat.APIs.Middlewares;
using LinkDev.Talabat.APIs.Service;
using LinkDev.Talabat.Core.Application;
using LinkDev.Talabat.Core.Application.Abstraction;
using LinkDev.Talabat.Core.Domain.Entities.Identity;
using LinkDev.Talabat.Infrastructure;
using LinkDev.Talabat.Infrastructure.Persistence;
using LinkDev.Talabat.Infrastructure.Persistence.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
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
                .AddApplicationPart(typeof(Controllers.AssemblyInformation).Assembly);  // Register Required Service by ASP.NET Core with APIs to DI Container.
           
          

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            webApplicationBuilder.Services.AddEndpointsApiExplorer().AddSwaggerGen();


            webApplicationBuilder.Services.AddHttpContextAccessor();
            webApplicationBuilder.Services.AddScoped(typeof(ILoggedInUserService),typeof(LoggedInUserService));


            webApplicationBuilder.Services.AddApplicationServices();
            webApplicationBuilder.Services.AddPersistenceServices(webApplicationBuilder.Configuration);
            webApplicationBuilder.Services.AddInfrastructureServices(webApplicationBuilder.Configuration);

            webApplicationBuilder.Services.AddIdentity<ApplicationUser, IdentityRole>((identityOptions) =>
            {
                identityOptions.SignIn.RequireConfirmedAccount = true;
                identityOptions.SignIn.RequireConfirmedEmail = true;
                identityOptions.SignIn.RequireConfirmedPhoneNumber = true;

                identityOptions.Password.RequireNonAlphanumeric = true;
                identityOptions.Password.RequiredUniqueChars = 2; //$M@%
                identityOptions.Password.RequiredLength = 6;
                identityOptions.Password.RequireDigit = true;
                identityOptions.Password.RequireLowercase = true;
                identityOptions.Password.RequireUppercase = true;

                identityOptions.User.RequireUniqueEmail = true;
                //identityOptions.User.AllowedUserNameCharacters="abcdenkotlog93124568_.+@*$";

                identityOptions.Lockout.AllowedForNewUsers = true;
                identityOptions.Lockout.MaxFailedAccessAttempts =5;
                identityOptions.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromHours(12);

                //identityOptions.Stores
                //identityOptions.Tokens
                //identityOptions.ClaimsIdentity
            })
                .AddEntityFrameworkStores<StoreIdentityDbContext>();


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

          await  app.InitializerDbAsync();

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
