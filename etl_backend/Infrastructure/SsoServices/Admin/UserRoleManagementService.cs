using Application.Dtos;
using Application.Services.Abstractions;
using Infrastructure.Dtos;
using Infrastructure.SsoServices.Admin.Abstractions;
using Infrastructure.SsoServices.Admin.Mappers;
using Infrastructure.SsoServices.User.Abstractions;

namespace Infrastructure.SsoServices.Admin;

public class UserRoleManagementService : IUserRoleManagementService
{
    private readonly ISsoClient _ssoClient;
    private readonly IRoleManagerService _roleManager;
    private readonly IKeycloakServiceAccountTokenProvider _accountTokenProvider;

    public UserRoleManagementService(
        ISsoClient ssoClient,
        IRoleManagerService roleManager,
        IKeycloakServiceAccountTokenProvider accountTokenProvider)
    {
        _ssoClient = ssoClient;
        _roleManager = roleManager;
        _accountTokenProvider = accountTokenProvider;
    }

    private string UsersEndpoint => "users"; 
    

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
}