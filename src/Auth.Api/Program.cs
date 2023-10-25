using Auth.Api.Interfaces;
using Auth.Api.Services;
using Common.Options;
using Common.Extensions;
using Serilog;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddAutoMapper(typeof(Program));

        var obj = builder.Services.AddOptions<AuthenticationSettings>().BindConfiguration("Authentication").ValidateDataAnnotations().ValidateOnStart();

        var authenticationSettings = builder.Configuration.GetSection("Authentication").Get<AuthenticationSettings>();

        if (authenticationSettings is not null)
        {
            switch (authenticationSettings.Provider)
            {
                case Common.AuthProvider.NA:
                    break;
                case Common.AuthProvider.JWT:
                    builder.Services.AddSingleton<IAuthService, JwtAuthService>();
                    break;
                case Common.AuthProvider.AWS_Cognito:
                    builder.Services.AddSingleton<IAuthService, AWSCognitoAuthService>();
                    break;
                default:
                    throw new InvalidDataException($"Invalid authentication provider defined in appsettings.json file.");
            }
        }

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseCustomMiddlewares();

        app.UseDeveloperExceptionPage();
        app.UseSerilogRequestLogging();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}