
using MagicVilla_VillaAPI.logging;

namespace MagicVilla_VillaAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            //Log.Logger = new LoggerConfiguration().MinimumLevel.Debug()
            //    .WriteTo.File("log/Villalogs.exe",rollingInterval: RollingInterval.Day).CreateLogger();

            //builder.Host.UseSerilog();

            builder.Services.AddControllers(option =>
            {
                //option.ReturnHttpNotAcceptable = true;
            }
            ).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();
            builder.Services.AddSwaggerGen();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSingleton < ILogging, LoggingV2>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
