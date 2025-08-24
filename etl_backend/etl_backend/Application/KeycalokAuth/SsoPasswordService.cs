using etl_backend.Application.KeycalokAuth.Abstraction;

namespace etl_backend.Application.KeycalokAuth;
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




