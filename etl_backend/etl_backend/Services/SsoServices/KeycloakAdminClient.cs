using etl_backend.Configuration;
using etl_backend.Services.Abstraction;
using etl_backend.Services.Abstraction.SsoServices;
using etl_backend.Services.Dtos;
using Microsoft.Extensions.Options;

namespace etl_backend.Services.SsoServices;

public class KeycloakAdminClient : IKeycloakAdminClient
{
    private readonly ISsoClient _ssoClient;
    private readonly KeycloakOptions _options;
    private readonly IRoleManagerService _roleManager;

    public KeycloakAdminClient(
        ISsoClient ssoClient,
        IOptions<KeycloakOptions> options,
        IRoleManagerService roleManager)
    {
        _ssoClient = ssoClient;
        _options = options.Value;
        _roleManager = roleManager;
    }

    private string UsersEndpoint => $"{_options.AuthServerUrl}/admin/realms/{_options.Realm}/users";

    public async Task<IEnumerable<UserWithRolesDto>> GetAllUsersAsync(string accessToken, CancellationToken cancellationToken)
    {
        var users = await _ssoClient.GetAsync<List<UserDto>>(UsersEndpoint, accessToken, cancellationToken);

        var result = new List<UserWithRolesDto>();
        foreach (var user in users)
        {
            var roles = await _roleManager.GetUserRolesAsync(user.Id, accessToken, cancellationToken);
            result.Add(new UserWithRolesDto(user, roles));
        }

        return result;
    }

    public async Task<UserWithRolesDto> GetUserByIdAsync(string userId, string accessToken, CancellationToken cancellationToken)
    {
        var user = await _ssoClient.GetAsync<UserDto>($"{UsersEndpoint}/{userId}", accessToken, cancellationToken);
        var roles = await _roleManager.GetUserRolesAsync(user.Id, accessToken, cancellationToken);

        return new UserWithRolesDto(user, roles);
    }

    public async Task CreateUserAsync(UserDto newUser, IEnumerable<string> roles, string accessToken, CancellationToken cancellationToken)
    {
        var createdUser = await _ssoClient.PostAsync<UserDto>(UsersEndpoint, newUser, accessToken, cancellationToken);
        if (createdUser == null) throw new Exception("Failed to read created user");

        if (roles.Any())
        {
            await _roleManager.AssignRolesToUserAsync(createdUser.Id, roles, accessToken, cancellationToken);
        }
    }

    public async Task EditUserAsync(string userId, UserDto updatedUser, IEnumerable<string>? roles, string accessToken, CancellationToken cancellationToken)
    {
        await _ssoClient.PutAsync($"{UsersEndpoint}/{userId}", updatedUser, accessToken, cancellationToken);

        if (roles != null)
        {
            var currentRoles = await _roleManager.GetUserRolesAsync(userId, accessToken, cancellationToken);

            var rolesToAdd = roles.Except(currentRoles);
            var rolesToRemove = currentRoles.Except(roles);

            if (rolesToAdd.Any())
                await _roleManager.AssignRolesToUserAsync(userId, rolesToAdd, accessToken, cancellationToken);

            if (rolesToRemove.Any())
                await _roleManager.RemoveRolesFromUserAsync(userId, rolesToRemove, accessToken, cancellationToken);
        }
    }

    public async Task DeleteUserAsync(string userId, string accessToken, CancellationToken cancellationToken)
    {
        await _ssoClient.DeleteAsync($"{UsersEndpoint}/{userId}", accessToken, cancellationToken);
    }
}
