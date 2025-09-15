using Application.Dtos;
using Application.Users.GetUserById.ServiceAbstractions;
using Infrastructure.SsoServices.Admin.Abstractions;
using Infrastructure.SsoServices.Admin.Mappers;
using Infrastructure.SsoServices.User.Abstractions;

namespace Infrastructure.SsoServices.Admin;

public class GetUserByIdService : IGetUserByIdService
{
    private readonly ISsoClient _ssoClient;
    private readonly IRoleManagerService _roleManager;
    private readonly IKeycloakServiceAccountTokenProvider _accountTokenProvider;

    public GetUserByIdService(
        ISsoClient ssoClient,
        IRoleManagerService roleManager,
        IKeycloakServiceAccountTokenProvider accountTokenProvider)
    {
        _ssoClient = ssoClient;
        _roleManager = roleManager;
        _accountTokenProvider = accountTokenProvider;
    }

    private string UsersEndpoint => "users"; 
    public async Task<UserWithRolesDto> GetUserByIdAsync(string userId, CancellationToken cancellationToken)
    {
        var accessToken = await _accountTokenProvider.GetServiceAccountTokenAsync();
        var userJson = await _ssoClient.GetAsync($"{UsersEndpoint}/{userId}", accessToken!, cancellationToken);

        var user = UserMapper.FromJsonElement(userJson.RootElement);
        var roles = await _roleManager.GetUserRolesAsync(user.Id, accessToken!, cancellationToken);

        return UserMapper.ToUserWithRolesDto(user, roles);
    }
    
}