using System.Security.Claims;
using etl_backend.Services.Abstraction.SsoServices;
using etl_backend.Services.Auth.Abstraction;
using etl_backend.Services.Dtos;
using etl_backend.Services.Auth.keycloakService.Abstraction;
using Newtonsoft.Json.Linq;

namespace etl_backend.Services.Auth.keycloakService;

public class KeycloakRoleExtractor : IRoleExtractor
{
    private readonly IRoleManagerService _roleManagerService;
    private readonly IKeycloakServiceAccountTokenProvider _tokenProvider;

    public KeycloakRoleExtractor(
        IRoleManagerService roleManagerService,
        IKeycloakServiceAccountTokenProvider tokenProvider)
    {
        _roleManagerService = roleManagerService;
        _tokenProvider = tokenProvider;
    }

    public async Task<IEnumerable<RoleDto>> ExtractRoles(ClaimsPrincipal principal, string scope, string key)
    {
        var claim = principal.FindFirst(scope);
        if (claim == null) return Enumerable.Empty<RoleDto>();

        try
        {
            var obj = JObject.Parse(claim.Value);
            var roleNames = obj[key]?.ToObject<List<string>>() ?? new();
            if (!roleNames.Any()) return Enumerable.Empty<RoleDto>();
            
            var token = await _tokenProvider.GetServiceAccountTokenAsync();
            if (string.IsNullOrEmpty(token)) return Enumerable.Empty<RoleDto>();

            
            var allRoles = _roleManagerService.GetAllRolesAsync(token!, CancellationToken.None)
                .GetAwaiter().GetResult();
            
            var matchedRoles = allRoles
                .Where(r => roleNames.Contains(r.Name))
                .ToList();

            return matchedRoles;
        }
        catch
        {
            return Enumerable.Empty<RoleDto>();
        }
    }
}