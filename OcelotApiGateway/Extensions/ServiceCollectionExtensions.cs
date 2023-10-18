using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ocelot.Authorization;
using OcelotApiGateway.Decorators;
using System.Text;

namespace OcelotApiGateway.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection DecorateClaimAuthoriser(this IServiceCollection services)
        {
            var serviceDescriptor = services.First(x => x.ServiceType == typeof(IClaimsAuthorizer));
            services.Remove(serviceDescriptor);

            var newServiceDescriptor = new ServiceDescriptor(serviceDescriptor.ImplementationType, serviceDescriptor.ImplementationType, serviceDescriptor.Lifetime);
            services.Add(newServiceDescriptor);

            services.AddTransient<IClaimsAuthorizer, ClaimAuthorizerDecorator>();

            return services;
        }

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

                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.Authority = configuration["Authentication:AWSCognito:Authority"];                    
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,

                        //ToDo Validate audience should be true
                        //
                        // Summary:
                        //     Gets or sets a boolean to control if the audience will be validated during token
                        //     validation.
                        //
                        // Remarks:
                        //     Validation of the audience, mitigates forwarding attacks. For example, a site
                        //     that receives a token, could not replay it to another side. A forwarded token
                        //     would contain the audience of the original site. This boolean only applies to
                        //     default audience validation. If Microsoft.IdentityModel.Tokens.TokenValidationParameters.AudienceValidator
                        //     is set, it will be called regardless of whether this property is true or false.
                        //     The default is true.
                        ValidateAudience = false,
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
