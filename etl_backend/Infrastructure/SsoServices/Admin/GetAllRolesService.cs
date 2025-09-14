using Application.Dtos;
using Application.Users.GetAllRoles.ServiceAbstractions;
using Infrastructure.SsoServices.Admin.Abstractions;
using Infrastructure.SsoServices.User.Abstractions;

namespace Infrastructure.SsoServices.Admin;

public class GetAllRolesService : IGetAllRoles
{
    private readonly IRoleManagerService _roleManager;
    private readonly IKeycloakServiceAccountTokenProvider _accountTokenProvider;

    public GetAllRolesService(
        IRoleManagerService roleManager,
        IKeycloakServiceAccountTokenProvider accountTokenProvider)
    {
        _roleManager = roleManager;
        _accountTokenProvider = accountTokenProvider;
    }

    public async Task<IEnumerable<RoleDto>> GetAllRolesAsync(CancellationToken cancellationToken)
    {
        var accessToken = await _accountTokenProvider.GetServiceAccountTokenAsync();
        return await _roleManager.GetAllRolesAsync(accessToken!, cancellationToken);
    }
}