using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using etl_backend.Services.Abstraction;
using Newtonsoft.Json.Linq;

namespace etl_backend.Services;

public class JwtRoleMapper : IRoleMapper
{
    readonly ILogger<JwtRoleMapper> _log;
    public JwtRoleMapper(ILogger<JwtRoleMapper> log) => _log = log;

    public Task MapRolesAsync(
        ClaimsPrincipal principal,
        string? rawToken = null,
        CancellationToken ct = default)
    {
        if (principal?.Identity is not ClaimsIdentity identity)
            return Task.CompletedTask;

        try
        {
            var jwt = ExtractJwtToken(principal, rawToken);
            if (jwt == null)
            {
                _log.LogDebug("No JwtSecurityToken available to map roles.");
                return Task.CompletedTask;
            }

            MapRealmRoles(identity, jwt);
            MapResourceRoles(identity, jwt);
        }
        catch (Exception ex)
        {
            _log.LogWarning(ex, "Failed to map roles from token.");
        }

        return Task.CompletedTask;
    }

    private JwtSecurityToken? ExtractJwtToken(ClaimsPrincipal principal, string? rawToken)
    {
        JwtSecurityToken? jwt = null;
        var handler = new JwtSecurityTokenHandler();

        // 1) Try claims
        var tokenClaim = principal.Claims.FirstOrDefault(c => c.Type == "jwt" || c.Type == "access_token");

        if (!string.IsNullOrEmpty(tokenClaim?.Value))
            jwt = handler.ReadJwtToken(tokenClaim.Value);

        // 2) Fallback to raw token
        if (jwt == null && !string.IsNullOrEmpty(rawToken))
            jwt = handler.ReadJwtToken(rawToken);

        return jwt;
    }

    private void MapRealmRoles(ClaimsIdentity identity, JwtSecurityToken jwt)
    {
        if (!jwt.Payload.TryGetValue("realm_access", out var realmAccessObj))
            return;

        JArray? rolesArray = null;

        switch (realmAccessObj)
        {
            case JObject jObj:
                rolesArray = jObj["roles"] as JArray;
                break;

            case IDictionary<string, object> dict:
                if (dict.TryGetValue("roles", out var rolesObj) &&
                    rolesObj is IEnumerable<object> enumerable)
                {
                    foreach (var r in enumerable)
                        AddRole(identity, r?.ToString());
                }

                break;

            case string jsonString:
                var parsed = JObject.Parse(jsonString);
                rolesArray = parsed["roles"] as JArray;
                break;
        }

        if (rolesArray != null)
        {
            foreach (var r in rolesArray)
                AddRole(identity, r?.ToString());
        }
    }

    private void MapResourceRoles(ClaimsIdentity identity, JwtSecurityToken jwt)
    {
        if (!jwt.Payload.TryGetValue("resource_access", out var resourceAccessObj))
            return;

        JObject? resObj = null;

        if (resourceAccessObj is JObject obj)
            resObj = obj;

        else if (resourceAccessObj is string json)
            resObj = JObject.Parse(json);

        if (resObj == null) return;

        foreach (var prop in resObj.Properties())
        {
            var roles = prop.Value["roles"] as JArray;
            if (roles == null) continue;

            foreach (var r in roles)
                AddRole(identity, r?.ToString());
        }
    }

    private static void AddRole(ClaimsIdentity identity, string? role)
    {
        if (!string.IsNullOrWhiteSpace(role))
            identity.AddClaim(new Claim(ClaimTypes.Role, role));
    }
}