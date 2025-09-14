using System.Text.Json;
using Application.Dtos;
using Infrastructure.SsoServices.Admin.Abstractions;
using Infrastructure.SsoServices.Admin.Mappers;
using Infrastructure.SsoServices.User.Abstractions;

namespace Infrastructure.SsoServices.Admin;

public class KeycloakClientRoleManagerService : IRoleManagerService
{
    private readonly ISsoClient _ssoClient;
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false,
        PropertyNameCaseInsensitive = false
    };

    public KeycloakClientRoleManagerService(ISsoClient ssoClient)
    {
        _ssoClient = ssoClient;
    }

    private const string UsersEndpoint = "users";
    private const string RolesEndpoint = "roles";

    public async Task<IEnumerable<RoleDto>> GetUserRolesAsync(
        string userId,
        string accessToken,
        CancellationToken cancellationToken)
    {
        var response = await _ssoClient.GetAsync(
            $"{UsersEndpoint}/{userId}/role-mappings/realm",
            accessToken,
            cancellationToken);

        return RoleMapper.FromJsonArray(response.RootElement);
    }

    public async Task AssignRolesToUserAsync(
        string userId,
        IEnumerable<RoleDto> roles,
        string accessToken,
        CancellationToken cancellationToken)
    {
        if (!roles.Any()) return;

        string jsonPayload = JsonSerializer.Serialize(roles, _jsonOptions);

        var elements = JsonSerializer.Deserialize<JsonElement[]>(jsonPayload, _jsonOptions) ?? [];
        
        await _ssoClient.PostAsync(
            $"{UsersEndpoint}/{userId}/role-mappings/realm",
            elements, 
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

        string jsonPayload = JsonSerializer.Serialize(roles, _jsonOptions);
        var elements = JsonSerializer.Deserialize<JsonElement[]>(jsonPayload, _jsonOptions) ?? [];

        await _ssoClient.DeleteAsync(
            $"{UsersEndpoint}/{userId}/role-mappings/realm",
            elements, 
            accessToken,
            cancellationToken);
    }

    public async Task<IEnumerable<RoleDto>> GetAllRolesAsync(
        string accessToken,
        CancellationToken cancellationToken)
    {
        var response = await _ssoClient.GetAsync(RolesEndpoint, accessToken, cancellationToken);

        return RoleMapper.FromJsonArray(response.RootElement);
    }
}
