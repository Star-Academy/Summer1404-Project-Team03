using System.Security.Claims;
using etl_backend.Services.Auth.Abstraction;
using Newtonsoft.Json.Linq;

namespace etl_backend.Services.Auth.keycloakService;

public class KeycloakRoleExtractor : IRoleExtractor
{
    public IEnumerable<string> ExtractRoles(ClaimsPrincipal principal, string scope, string key)
    {
        var claim = principal.FindFirst(scope);
        if (claim == null) return Enumerable.Empty<string>();

        try
        {
            var obj = JObject.Parse(claim.Value);
            var roles = obj[key]?.ToObject<List<string>>() ?? new();
            return roles;
        }
        catch
        {
            return Enumerable.Empty<string>();
        }
    }
}