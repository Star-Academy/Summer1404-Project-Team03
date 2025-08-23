using etl_backend.Services.Abstraction;
using etl_backend.Services.Abstraction.SsoServices;
using etl_backend.Services.Dtos;

namespace etl_backend.Services
{
    public class SsoProfileService : ISsoProfileService
    {
        private readonly ISsoClient _ssoClient;

        public SsoProfileService(ISsoClient ssoClient)
        {
            _ssoClient = ssoClient;
        }

        public async Task<UserProfile> GetProfileAsync(string accessToken, CancellationToken cancellationToken = default)
        {
            return await _ssoClient.GetAsync<UserProfile>(
                "protocol/openid-connect/userinfo",
                accessToken,
                cancellationToken
            );
        }
    }
}