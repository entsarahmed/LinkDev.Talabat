
namespace LinkDev.Talabat.APIs
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var webApplicationBuilder = WebApplication.CreateBuilder(args);

            #region Configure Services
            // Add services to the container.

            webApplicationBuilder.Services.AddControllers();// Register Required Service by ASP.NET Core with APIs to DI Container.
                                                            // 


            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            webApplicationBuilder.Services.AddEndpointsApiExplorer();
            webApplicationBuilder.Services.AddSwaggerGen();

            #endregion


            #region Cofigure Kestrel Middleware
            var app = webApplicationBuilder.Build();

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
