using etl_backend.Services.Auth.Abstraction;
using etl_backend.Services.Auth.keycloakAuthService.Dtos;

namespace etl_backend.Services.Auth;

public class TokenCookieService : ITokenCookieService
{
    private const string AccessCookieName = "access_token";
    private const string RefreshCookieName = "refresh_token";

    public void SetTokens(HttpResponse response, TokenResponseDto tokenResponse)
    {
        var now = DateTimeOffset.UtcNow;

        var accessOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = now.AddSeconds(tokenResponse.ExpiresIn)
        };

        var refreshOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = now.AddSeconds(tokenResponse.RefreshExpiresIn)
        };

        response.Cookies.Append(AccessCookieName, tokenResponse.AccessToken, accessOptions);
        response.Cookies.Append(RefreshCookieName, tokenResponse.RefreshToken, refreshOptions);
    }

    public void DeleteTokens(HttpResponse response)
    {
        response.Cookies.Delete(AccessCookieName);
        response.Cookies.Delete(RefreshCookieName);
    }
}

