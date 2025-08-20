using etl_backend.Configuration;
using etl_backend.Services.Abstraction;
using Microsoft.Extensions.Options;

namespace etl_backend.Services.SsoServices;

public class KeycloakClientRoleManagerService : IRoleManagerService
{
    private readonly ISsoClient _ssoClient;
    private readonly KeycloakOptions _options;

    public KeycloakClientRoleManagerService(ISsoClient ssoClient, IOptions<KeycloakOptions> options)
    {
        _ssoClient = ssoClient;
        _options = options.Value;
    }

    private string UsersEndpoint => $"admin/realms/{_options.Realm}/users";

    public async Task<IEnumerable<string>> GetUserRolesAsync(string userId, string accessToken, CancellationToken cancellationToken)
    {
        var roles = await _ssoClient.GetAsync<List<RoleDto>>(
            $"{UsersEndpoint}/{userId}/role-mappings/realm",
            accessToken,
            cancellationToken);

        return roles?.Select(r => r.Name) ?? Array.Empty<string>();
    }

    public async Task AssignRolesToUserAsync(string userId, IEnumerable<string> roles, string accessToken, CancellationToken cancellationToken)
    {
        if (!roles.Any()) return;

        var roleDtos = roles.Select(r => new RoleDto { Name = r }).ToList();
        await _ssoClient.PostAsync($"{UsersEndpoint}/{userId}/role-mappings/realm", roleDtos, accessToken, cancellationToken);
    }

    public async Task RemoveRolesFromUserAsync(string userId, IEnumerable<string> roles, string accessToken, CancellationToken cancellationToken)
    {
        if (!roles.Any()) return;

        var roleDtos = roles.Select(r => new RoleDto { Name = r }).ToList();
        await _ssoClient.DeleteAsync($"{UsersEndpoint}/{userId}/role-mappings/realm", accessToken, cancellationToken);
    }
}