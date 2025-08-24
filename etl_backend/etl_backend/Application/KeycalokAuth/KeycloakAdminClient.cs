using etl_backend.Application.Dtos;
using etl_backend.Application.KeycalokAuth.Abstraction;
using etl_backend.Application.KeycalokAuth.Dtos;

namespace etl_backend.Application.KeycalokAuth;

public class KeycloakAdminClient : IKeycloakAdminClient
{
    private readonly ISsoClient _ssoClient;
    private readonly IRoleManagerService _roleManager;
    private readonly IKeycloakServiceAccountTokenProvider _accountTokenProvider;

    public KeycloakAdminClient(
        ISsoClient ssoClient,
        IRoleManagerService roleManager, IKeycloakServiceAccountTokenProvider accountTokenProvider)
    {
        _ssoClient = ssoClient;
        _roleManager = roleManager;
        _accountTokenProvider = accountTokenProvider;
    }

    private string UsersEndpoint => "/users/";

    public async Task<IEnumerable<UserWithRolesDto>> GetAllUsersAsync(CancellationToken cancellationToken)
    {
        var accessToken = await _accountTokenProvider.GetServiceAccountTokenAsync();
        var usersJsonDoc = await _ssoClient.GetAsync(UsersEndpoint, accessToken!, cancellationToken);

        var users = new List<UserDto>();

        foreach (var element in usersJsonDoc.RootElement.EnumerateArray())
        {
            var user = new UserDto
            {
                Id = element.GetProperty("id").GetString()!,
                Username = element.GetProperty("username").GetString() ?? string.Empty,
                Email = element.GetProperty("email").GetString() ?? string.Empty,
                FirstName = element.TryGetProperty("firstName", out var fn) ? fn.GetString() : null,
                LastName = element.TryGetProperty("lastName", out var ln) ? ln.GetString() : null
            };

            users.Add(user);
        }

        var result = new List<UserWithRolesDto>();

        foreach (var user in users)
        {
            var roles = await _roleManager.GetUserRolesAsync(user.Id, accessToken!, cancellationToken);
            result.Add(new UserWithRolesDto(user, roles));
        }

        return result;
    }

    public async Task<UserWithRolesDto> GetUserByIdAsync(string userId, CancellationToken cancellationToken)
    {
        var accessToken = await _accountTokenProvider.GetServiceAccountTokenAsync();
        var userJson = await _ssoClient.GetAsync($"{UsersEndpoint}/{userId}", accessToken!, cancellationToken);

        var element = userJson.RootElement;

        var user = new UserDto
        {
            Id = element.GetProperty("id").GetString()!,
            Username = element.GetProperty("username").GetString() ?? string.Empty,
            Email = element.GetProperty("email").GetString() ?? string.Empty,
            FirstName = element.TryGetProperty("firstName", out var fn) ? fn.GetString() : null,
            LastName = element.TryGetProperty("lastName", out var ln) ? ln.GetString() : null
        };

        var roles = await _roleManager.GetUserRolesAsync(user.Id, accessToken!, cancellationToken);

        return new UserWithRolesDto(user, roles);
    }

    public async Task<UserDto> CreateUserAsync(UserCreateDto newUser,
        CancellationToken cancellationToken)
    {
        var accessToken = await _accountTokenProvider.GetServiceAccountTokenAsync();

        var newUserCreadentials = new KeycloakCredentialDto
        {
            Temporary = true,
            Type = "password",
            Value = newUser.Password ?? newUser.Username
        };

        var userToCreate = new KeycloakUserCreateDto
        {
            Username = newUser.Username,
            Email = newUser.Email,
            FirstName = newUser.FirstName,
            LastName = newUser.LastName,
            Credentials = new List<KeycloakCredentialDto> { newUserCreadentials, }
        };
        
        var createdUserJson = await _ssoClient.PostAsync(UsersEndpoint, userToCreate, accessToken!, cancellationToken);

        // Map created user
        var element = createdUserJson.RootElement;
        var createdUser = new UserDto
        {
            Id = element.GetProperty("id").GetString()!,
            Username = element.GetProperty("username").GetString() ?? string.Empty,
            Email = element.GetProperty("email").GetString() ?? string.Empty,
            FirstName = element.TryGetProperty("firstName", out var fn) ? fn.GetString() : null,
            LastName = element.TryGetProperty("lastName", out var ln) ? ln.GetString() : null
        };

        return createdUser;
    }

    public async Task<UserDto> EditUserAsync(string userId, EditUserRequestDto userToUpdate,
        CancellationToken cancellationToken)
    {
        var accessToken = await _accountTokenProvider.GetServiceAccountTokenAsync();
        await _ssoClient.PutAsync($"{UsersEndpoint}/{userId}", userToUpdate, accessToken!, cancellationToken);
        var jsonUpdatedUserWithRoles = await GetUserByIdAsync(userId, cancellationToken);
        var updatedUser = new UserDto
        {
            Id = jsonUpdatedUserWithRoles.Id,
            Username = jsonUpdatedUserWithRoles.Username,
            Email = jsonUpdatedUserWithRoles.Email,
            FirstName = jsonUpdatedUserWithRoles.FirstName,
            LastName = jsonUpdatedUserWithRoles.LastName,
        };
        return updatedUser;
    }


    public async Task DeleteUserAsync(string userId, CancellationToken cancellationToken)
    {
        var accessToken = await _accountTokenProvider.GetServiceAccountTokenAsync();
        await _ssoClient.DeleteAsync($"{UsersEndpoint}/{userId}", null, accessToken!, cancellationToken: cancellationToken);
    }
    
    public async Task AddRolesToUserAsync(
        string userId,
        IEnumerable<RoleDto> roles,
        CancellationToken cancellationToken)
    {
        if (!roles.Any()) return;

        var accessToken = await _accountTokenProvider.GetServiceAccountTokenAsync();
        await _roleManager.AssignRolesToUserAsync(userId, roles, accessToken!, cancellationToken);
    }


    public async Task RemoveRolesFromUserAsync(
        string userId,
        IEnumerable<RoleDto> roles,
        CancellationToken cancellationToken)
    {
        if (!roles.Any()) return;

        var accessToken = await _accountTokenProvider.GetServiceAccountTokenAsync();
        await _roleManager.RemoveRolesFromUserAsync(userId, roles, accessToken!, cancellationToken);
    }

    public async Task<IEnumerable<RoleDto>> GetAllRolesAsync(CancellationToken cancellationToken)
    {
        var accessToken = await _accountTokenProvider.GetServiceAccountTokenAsync();
        var allRoles = await _roleManager.GetAllRolesAsync(accessToken!, cancellationToken);
        return allRoles;
    }
}