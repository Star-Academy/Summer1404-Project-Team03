using Application.Dtos;
using Application.Services.Abstractions;
using Infrastructure.Dtos;
using Infrastructure.SsoServices.Admin.Abstractions;
using Infrastructure.SsoServices.Admin.Mappers;
using Infrastructure.SsoServices.User.Abstractions;

namespace Infrastructure.SsoServices.Admin;

public class UserManagementService : IUserManagementService
{
    private readonly ISsoClient _ssoClient;
    private readonly IRoleManagerService _roleManager;
    private readonly IKeycloakServiceAccountTokenProvider _accountTokenProvider;

    public UserManagementService(
        ISsoClient ssoClient,
        IRoleManagerService roleManager,
        IKeycloakServiceAccountTokenProvider accountTokenProvider)
    {
        _ssoClient = ssoClient;
        _roleManager = roleManager;
        _accountTokenProvider = accountTokenProvider;
    }

    private string UsersEndpoint => "users"; 

    public async Task<IEnumerable<UserWithRolesDto>> GetAllUsersAsync(CancellationToken cancellationToken)
    {
        var accessToken = await _accountTokenProvider.GetServiceAccountTokenAsync();
        var usersJsonDoc = await _ssoClient.GetAsync(UsersEndpoint, accessToken!, cancellationToken);

        var users = usersJsonDoc.RootElement
            .EnumerateArray()
            .Select(UserMapper.FromJsonElement)
            .ToList();

        var result = new List<UserWithRolesDto>();

        foreach (var user in users)
        {
            var roles = await _roleManager.GetUserRolesAsync(user.Id, accessToken!, cancellationToken);
            result.Add(UserMapper.ToUserWithRolesDto(user, roles));
        }

        return result;
    }

    public async Task<UserWithRolesDto> GetUserByIdAsync(string userId, CancellationToken cancellationToken)
    {
        var accessToken = await _accountTokenProvider.GetServiceAccountTokenAsync();
        var userJson = await _ssoClient.GetAsync($"{UsersEndpoint}/{userId}", accessToken!, cancellationToken);

        var user = UserMapper.FromJsonElement(userJson.RootElement);
        var roles = await _roleManager.GetUserRolesAsync(user.Id, accessToken!, cancellationToken);

        return UserMapper.ToUserWithRolesDto(user, roles);
    }

    public async Task<UserDto> CreateUserAsync(UserCreateDto newUser, CancellationToken cancellationToken)
    {
        var accessToken = await _accountTokenProvider.GetServiceAccountTokenAsync();

        var userToCreate = UserMapper.ToKeycloakUserCreateDto(newUser);

        var createdUserJson = await _ssoClient.PostAsync(UsersEndpoint, userToCreate, accessToken!, cancellationToken);

        return UserMapper.FromJsonElement(createdUserJson.RootElement);
    }

    public async Task<UserDto> EditUserAsync(string userId, EditUserRequestDto userToUpdate, CancellationToken cancellationToken)
    {
        var accessToken = await _accountTokenProvider.GetServiceAccountTokenAsync();

        var jsonElement = UserMapper.ToJsonElement(userToUpdate);

        await _ssoClient.PutAsync($"{UsersEndpoint}/{userId}", jsonElement, accessToken!, cancellationToken);

        var updatedUserWithRoles = await GetUserByIdAsync(userId, cancellationToken);
        return updatedUserWithRoles.ToUserDto();
    }

    public async Task DeleteUserAsync(string userId, CancellationToken cancellationToken)
    {
        var accessToken = await _accountTokenProvider.GetServiceAccountTokenAsync();
        await _ssoClient.DeleteAsync($"{UsersEndpoint}/{userId}", null, accessToken!, cancellationToken);
    }

    public async Task AddRolesToUserAsync(
        string userId,
        RoleDto[] roles,
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
        return await _roleManager.GetAllRolesAsync(accessToken!, cancellationToken);
    }
}