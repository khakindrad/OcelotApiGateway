using Common.Extensions;
using Common.Options;
using Microsoft.AspNetCore.Builder;

namespace Common.Extensions
{
    public static class AppBuilderExtensions
    {
        public static IApplicationBuilder UseAuthentication(this IApplicationBuilder app, AuthenticationSettings? authenticationSettings)
        {
            if (authenticationSettings is not null)
            {
                switch (authenticationSettings.Provider)
                {
                    case AuthProvider.NA:
                        break;
                    case AuthProvider.JWT:
                    case AuthProvider.AWS_Cognito:
                        app.UseAuthentication();
                        break;
                    default:
                        throw new InvalidDataException($"Invalid authentication provider defined in appsettings.json file.");
                }
            }

            return app;
        }
    }
}