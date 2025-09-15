using Application.Users.DeleteUser.ServiceAbstractions;
using Infrastructure.SsoServices.Admin.Abstractions;
using Infrastructure.SsoServices.User.Abstractions;

namespace Infrastructure.SsoServices.Admin;

public class DeleteUserService : IDeleteUserService
{
    private readonly ISsoClient _ssoClient;
    private readonly IKeycloakServiceAccountTokenProvider _accountTokenProvider;

    public DeleteUserService(
        ISsoClient ssoClient,
        IKeycloakServiceAccountTokenProvider accountTokenProvider)
    {
        _ssoClient = ssoClient;
        _accountTokenProvider = accountTokenProvider;
    }

    private string UsersEndpoint => "users"; 
    public async Task DeleteUserAsync(string userId, CancellationToken cancellationToken)
    {
        var accessToken = await _accountTokenProvider.GetServiceAccountTokenAsync();
        await _ssoClient.DeleteAsync($"{UsersEndpoint}/{userId}", null, accessToken!, cancellationToken);
    }
}