using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using OcelotApiGateway.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot(builder.Configuration);
builder.Services.DecorateClaimAuthoriser();

var authProvider = builder.Configuration["Authentication:Provider"];

if (authProvider is not null)
{
    builder.Services.AddAuthentication(builder.Configuration, authProvider);
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseDeveloperExceptionPage();
app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

if (authProvider is not null)
{
    app.UseAuthentication();
}

app.UseAuthorization();

app.MapControllers();

await app.UseOcelot();

app.Run();
