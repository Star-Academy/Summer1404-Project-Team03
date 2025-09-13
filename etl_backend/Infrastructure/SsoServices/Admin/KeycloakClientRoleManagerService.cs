using Application.Dtos;
using Infrastructure.Dtos;
using Infrastructure.SsoServices.Admin.Abstractions;
using Infrastructure.SsoServices.User.Abstractions;

namespace Infrastructure.SsoServices.Admin;

public class KeycloakClientRoleManagerService : IRoleManagerService
{
    private readonly ISsoClient _ssoClient;

    public KeycloakClientRoleManagerService(ISsoClient ssoClient)
    {
        _ssoClient = ssoClient;
    }

    private string UsersEndpoint => $"users";

    
    public async Task<IEnumerable<RoleDto>> GetUserRolesAsync(
        string userId,
        string accessToken,
        CancellationToken cancellationToken)
    {
        var rolesJson = await _ssoClient.GetAsync(
            $"{UsersEndpoint}/{userId}/role-mappings/realm", 
            accessToken, 
            cancellationToken);

        var roles = rolesJson.RootElement.EnumerateArray()
            .Select(r => new RoleDto
            {
                Id = r.GetProperty("id").GetString() ?? string.Empty,
                Name = r.GetProperty("name").GetString() ?? string.Empty
            })
            .Where(r => !string.IsNullOrEmpty(r.Id) && !string.IsNullOrEmpty(r.Name));

        return roles;
    }
    
    public async Task AssignRolesToUserAsync(
        string userId,
        IEnumerable<RoleDto> roles,
        string accessToken,
        CancellationToken cancellationToken)
    {
        if (!roles.Any()) return;

        await _ssoClient.PostAsync(
            $"{UsersEndpoint}/{userId}/role-mappings/realm",
            roles,
            accessToken,
            cancellationToken);
    }


    public async Task RemoveRolesFromUserAsync(
        string userId,
        IEnumerable<RoleDto> roles,
        string accessToken,
        CancellationToken cancellationToken)
    {
        if (!roles.Any()) return;

        await _ssoClient.DeleteAsync(
            $"{UsersEndpoint}/{userId}/role-mappings/realm",
            roles,
            accessToken,
            cancellationToken);
    }

    
    public async Task<IEnumerable<RoleDto>> GetAllRolesAsync(
        string accessToken,
        CancellationToken cancellationToken)
    {
        var rolesJson = await _ssoClient.GetAsync(
            $"roles",
            accessToken,
            cancellationToken);

        var roles = rolesJson.RootElement.EnumerateArray()
            .Select(r => new RoleDto
            {
                Id = r.GetProperty("id").GetString() ?? string.Empty,
                Name = r.GetProperty("name").GetString() ?? string.Empty
            })
            .Where(r => !string.IsNullOrEmpty(r.Id) && !string.IsNullOrEmpty(r.Name));

        return roles;
    }
}
