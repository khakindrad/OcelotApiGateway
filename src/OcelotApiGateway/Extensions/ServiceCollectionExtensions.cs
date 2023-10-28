using Ocelot.Authorization;
using OcelotApiGateway.Decorators;

namespace OcelotApiGateway.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection DecorateClaimAuthoriser(this IServiceCollection services)
    {
        var serviceDescriptor = services.First(x => x.ServiceType == typeof(IClaimsAuthorizer));
        services.Remove(serviceDescriptor);

        if (serviceDescriptor is not null && serviceDescriptor.ImplementationType is not null)
        {
            var newServiceDescriptor = new ServiceDescriptor(serviceDescriptor.ImplementationType, serviceDescriptor.ImplementationType, serviceDescriptor.Lifetime);
            services.Add(newServiceDescriptor);
        }

        services.AddTransient<IClaimsAuthorizer, ClaimAuthorizerDecorator>();

        return services;
    }
}
