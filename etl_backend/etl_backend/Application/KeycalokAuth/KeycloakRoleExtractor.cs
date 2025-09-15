using System.Security.Claims;
using etl_backend.Application.Abstraction;
using etl_backend.Application.Dtos;
using etl_backend.Application.KeycalokAuth.Abstraction;
using Newtonsoft.Json.Linq;

namespace etl_backend.Application.KeycalokAuth;

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

    public async Task<IEnumerable<RoleDto>> ExtractRoles(ClaimsPrincipal user, string scope, string key)
    {
        string? userId = user.FindFirst("id")?.Value
                     ?? user.FindFirst(c => c.Type.EndsWith("nameidentifier"))?.Value;
        
        var token = await _tokenProvider.GetServiceAccountTokenAsync();

        
        var userRoles = await _roleManagerService.GetUserRolesAsync(
            userId ?? throw new InvalidOperationException(nameof(userId)),
            token ?? throw new InvalidOperationException(nameof(token)),
            CancellationToken.None);
        return userRoles;
    }
}