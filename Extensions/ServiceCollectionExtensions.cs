using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration, string authProvider)
        {
            if (authProvider == "JWT")
            {
                services.AddAuthentication(opt =>
                {
                    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                    .AddJwtBearer(options =>
                    {
                        options.SaveToken = true;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = false,
                            ValidateIssuerSigningKey = true,
                            ValidAudience = configuration["Authentication:JWT:ValidAudience"],
                            ValidIssuer = configuration["Authentication:JWT:ValidIssuer"],
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Authentication:JWT:Secret"]))
                        };
                    });
            }
            else if (authProvider == "AWSCognito")
            {
                // https://codewithmukesh.com/blog/securing-dotnet-webapi-with-amazon-cognito/
                services.AddCognitoIdentity();

                services.AddAuthorization(options =>
                {
                    options.AddPolicy("SuperAdminOnly", policy => policy.RequireClaim("cognito:groups", "SuperAdmin"));
                });

                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    //options.SaveToken = true;
                    options.Authority = configuration["Authentication:AWSCognito:Authority"];
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,

                        ValidateAudience = true,
                        ValidAudience = configuration["Authentication:AWSCognito:ValidAudience"]
                    };
                });
            }
            else
            {
                throw new InvalidDataException($"Invalid authentication provider defined in appsettings.json file.");
            }

            return services;
        }

    }
}