using etl_backend.Application.KeycalokAuth.Abstraction;
using etl_backend.Application.KeycalokAuth.Dtos;
using etl_backend.Application.UsersAuth.Abstraction;
using Microsoft.Extensions.Options;

namespace etl_backend.Application.KeycalokAuth;

public class KeycloakTokenHandler : IKeycloakTokenHandler
{
    private readonly ITokenExtractor _extractor;
    private readonly ITokenCookieService _cookieService;
    private readonly ITokenExpirationChecker _tokenChecker;
    private readonly IAccessTokenRefreshable _accessTokenRefresher;
    private readonly KeycloakOptions _keycloakOptions;
    private readonly TimeSpan _clockSkew = TimeSpan.FromMinutes(2);

    public KeycloakTokenHandler(
        ITokenExtractor extractor,
        ITokenCookieService cookieService,
        ITokenExpirationChecker tokenChecker,
        IAccessTokenRefreshable accessTokenRefresher, IOptions<KeycloakOptions> keycloakOptions)
    {
        _extractor = extractor;
        _cookieService = cookieService;
        _tokenChecker = tokenChecker;
        _accessTokenRefresher = accessTokenRefresher;
        _keycloakOptions = keycloakOptions.Value;
    }

    public async Task<string?> GetOrRefreshAccessTokenAsync(HttpContext context, CancellationToken cancellationToken)
    {
        var accessToken = _extractor.GetAccessToken(context.Request, _keycloakOptions.AccessCookieName);

        if (!string.IsNullOrEmpty(accessToken))
        {
            if (!_tokenChecker.IsAccessTokenExpired(accessToken, _clockSkew, _keycloakOptions.ExpClaimType))
                return accessToken;

            var refreshToken = _extractor.GetRefreshToken(context.Request, _keycloakOptions.RefreshCookieName);
            if (!string.IsNullOrEmpty(refreshToken))
            {
                var newTokens = await _accessTokenRefresher.RefreshAccessTokenAsync(refreshToken, cancellationToken);
                if (newTokens != null)
                {
                    _cookieService.SetTokens(context.Response, newTokens);
                    return newTokens.AccessToken;
                }
            }
        }

        return null;
    }
}