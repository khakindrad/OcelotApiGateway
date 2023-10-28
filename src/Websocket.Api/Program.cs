using Serilog;
using Websocket.Api.Extensions;

namespace Websocket.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //Adding Strongly typed options
            builder.Services.RegisterCustomOptions();

            //Adding custom services
            builder.Services.RegisterCustomServices(builder.Configuration);

            var app = builder.Build();
            
            var logger = app.Services.GetRequiredService<ILogger<Program>>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseDeveloperExceptionPage();

            app.UseCustomMiddlewares();
            app.UseWebSocketServer(builder.Configuration, logger);
            app.UseServerHandler(builder.Configuration, logger);

            app.UseSerilogRequestLogging();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}