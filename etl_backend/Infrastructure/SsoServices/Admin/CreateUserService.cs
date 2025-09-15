using Application.Dtos;
using Application.Users.CreateUser.ServiceAbstractions;
using Infrastructure.SsoServices.Admin.Abstractions;
using Infrastructure.SsoServices.Admin.Mappers;
using Infrastructure.SsoServices.User.Abstractions;

namespace Infrastructure.SsoServices.Admin;

public class CreateUserService : ICreateUser
{
    private readonly ISsoClient _ssoClient;
    private readonly IKeycloakServiceAccountTokenProvider _accountTokenProvider;

    public CreateUserService(
        ISsoClient ssoClient,
        IKeycloakServiceAccountTokenProvider accountTokenProvider)
    {
        _ssoClient = ssoClient;
        _accountTokenProvider = accountTokenProvider;
    }

    private string UsersEndpoint => "users"; 
    public async Task<UserDto> CreateUserAsync(UserCreateDto newUser, CancellationToken cancellationToken)
    {
        var accessToken = await _accountTokenProvider.GetServiceAccountTokenAsync();

        var userToCreate = UserMapper.ToKeycloakUserCreateDto(newUser);

        var createdUserJson = await _ssoClient.PostAsync(UsersEndpoint, userToCreate, accessToken!, cancellationToken);

        return UserMapper.FromJsonElement(createdUserJson.RootElement);
    }
}