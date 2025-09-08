using Infrastructure.SsoServices.User.Abstractions;

namespace Infrastructure.SsoServices.User;
public class SsoPasswordService : ISsoPasswordService
{
    private readonly ISsoClient _ssoClient;

    public SsoPasswordService(ISsoClient ssoClient)
    {
        _ssoClient = ssoClient;
    }

    public async Task ChangeOwnPasswordAsync(string accessToken, string currentPassword, string newPassword, CancellationToken cancellationToken = default)
    {
        var body = new
        {
            currentPassword,
            newPassword
        };

        await _ssoClient.PostAsync("account/credentials/password", body, accessToken, cancellationToken);
    }
}




