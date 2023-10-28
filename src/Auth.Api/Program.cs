using Auth.Api.Interfaces;
using Auth.Api.Services;
using Common.Options;
using Common.Extensions;
using Serilog;
using Common;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddOptions<AuthenticationSettings>().BindConfiguration("Authentication").ValidateDataAnnotations().ValidateOnStart();

var authenticationSettings = builder.Configuration.GetSection("Authentication").Get<AuthenticationSettings>();

if (authenticationSettings is not null)
{
    switch (authenticationSettings.Provider)
    {
        case AuthProvider.NA:
            break;
        case AuthProvider.JWT:
            builder.Services.AddSingleton<IAuthService, JwtAuthService>();
            break;
        case AuthProvider.AWSCognito:
            builder.Services.AddSingleton<IAuthService, AwsCognitoAuthService>();
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

await app.RunAsync().ConfigureAwait(false);
