using Ocelot.Authorization;
using Ocelot.DownstreamRouteFinder.UrlMatcher;
using Ocelot.Responses;
using System.Security.Claims;

namespace OcelotApiGateway.Decorators;

public sealed class ClaimAuthorizerDecorator : IClaimsAuthorizer
{
    private readonly ClaimsAuthorizer _authoriser;

    public ClaimAuthorizerDecorator(ClaimsAuthorizer authoriser)
    {
        _authoriser = authoriser;
    }

    public Response<bool> Authorize(ClaimsPrincipal claimsPrincipal, Dictionary<string, string> routeClaimsRequirement, List<PlaceholderNameAndValue> urlPathPlaceholderNameAndValues)
    {
        var newRouteClaimsRequirement = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (var kvp in routeClaimsRequirement)
        {
            if (kvp.Key.Contains("?_?", StringComparison.OrdinalIgnoreCase))
            {
                var key = kvp.Key.Replace("?_?", ":", StringComparison.OrdinalIgnoreCase);
                newRouteClaimsRequirement.Add(key, kvp.Value);
            }
            else
            {
                newRouteClaimsRequirement.Add(kvp.Key, kvp.Value);
            }
        }

        return _authoriser.Authorize(claimsPrincipal, newRouteClaimsRequirement, urlPathPlaceholderNameAndValues);
    }
}
