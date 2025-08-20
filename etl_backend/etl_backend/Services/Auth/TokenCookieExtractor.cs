using etl_backend.Services.Auth.Abstraction;

namespace etl_backend.Services.Auth;

public class TokenCookieExtractor: ITokenExtractor
{
    private const string RefreshCookieName = "refresh_token";

    public string? GetAccessToken(HttpRequest request, string accessCookieName)
    {
        return request.Cookies.TryGetValue(accessCookieName, out var t) ? t : null;
    }

    public string? GetRefreshToken(HttpRequest request, string refreshCookieName)
    {
        return request.Cookies.TryGetValue(refreshCookieName, out var t) ? t : null;
    }   
}