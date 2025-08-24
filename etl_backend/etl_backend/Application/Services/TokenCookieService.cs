using etl_backend.Application.Abstraction;
using etl_backend.Application.KeycalokAuth.Dtos;

namespace etl_backend.Application.Services;

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
            SameSite = SameSiteMode.None,
            Expires = now.AddSeconds(tokenResponse.RefreshExpiresIn) 
            // access token should be in cookies as long as refresh
        };

        var refreshOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = now.AddSeconds(tokenResponse.RefreshExpiresIn)
        };

        response.Cookies.Append(AccessCookieName, tokenResponse.AccessToken, accessOptions);
        response.Cookies.Append(RefreshCookieName, tokenResponse.RefreshToken, refreshOptions);
    }

    public void RemoveTokens(HttpResponse response)
    {
        response.Cookies.Delete(AccessCookieName);
        response.Cookies.Delete(RefreshCookieName);
    }
}

