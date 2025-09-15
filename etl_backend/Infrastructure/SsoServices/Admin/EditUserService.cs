using Application.Dtos;
using Application.Users.EditUser.ServiceAbstractions;
using Application.Users.GetUserById.ServiceAbstractions;
using Infrastructure.SsoServices.Admin.Abstractions;
using Infrastructure.SsoServices.Admin.Mappers;
using Infrastructure.SsoServices.User.Abstractions;

namespace Infrastructure.SsoServices.Admin;

public class EditUserService : IAdminEditUserService
{
    private readonly ISsoClient _ssoClient;
    private readonly IKeycloakServiceAccountTokenProvider _accountTokenProvider;
    private readonly IGetUserByIdService _getUserByIdService;

    public EditUserService(
        ISsoClient ssoClient,
        IKeycloakServiceAccountTokenProvider accountTokenProvider, IGetUserByIdService getUserByIdService)
    {
        _ssoClient = ssoClient;
        _accountTokenProvider = accountTokenProvider;
        _getUserByIdService = getUserByIdService;
    }

    private string UsersEndpoint => "users"; 
    public async Task<UserDto> EditUserAsync(string userId, EditUserRequestDto userToUpdate, CancellationToken cancellationToken)
    {
        var accessToken = await _accountTokenProvider.GetServiceAccountTokenAsync();

        var jsonElement = UserMapper.ToJsonElement(userToUpdate);

        await _ssoClient.PutAsync($"{UsersEndpoint}/{userId}", jsonElement, accessToken!, cancellationToken);

        var updatedUserWithRoles = await _getUserByIdService.GetUserByIdAsync(userId, cancellationToken);
        return updatedUserWithRoles.ToUserDto();
    }
}