using Common.Extensions;
using Common.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Common.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAuthentication(this IServiceCollection services, AuthenticationSettings? authenticationSettings)
        {
            if (authenticationSettings is not null)
            {
                switch (authenticationSettings.Provider)
                {
                    case AuthProvider.NA:
                        break;
                    case AuthProvider.JWT:
                        {
                            if (authenticationSettings.JWT is null)
                            {
                                throw new ArgumentNullException(nameof(authenticationSettings.JWT));
                            }

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
                                    ValidAudience = authenticationSettings.ValidAudience,
                                    ValidIssuer = authenticationSettings.JWT.ValidIssuer,
                                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JWT.Secret))
                                };
                            });
                        }
                        break;
                    case AuthProvider.AWS_Cognito:
                        {
                            if (authenticationSettings.AWSCognito is null)
                            {
                                throw new ArgumentNullException(nameof(authenticationSettings.AWSCognito));
                            }

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
                                options.SaveToken = true;
                                options.Authority = authenticationSettings.AWSCognito.Authority;
                                options.TokenValidationParameters = new TokenValidationParameters
                                {
                                    ValidateIssuerSigningKey = true,

                                    ValidateAudience = true,
                                    ValidAudience = authenticationSettings.ValidAudience
                                };
                            });
                        }
                        break;
                    default:
                        throw new InvalidDataException($"Invalid authentication provider defined in appsettings.json file.");
                }
            }

            return services;
        }

    }
}