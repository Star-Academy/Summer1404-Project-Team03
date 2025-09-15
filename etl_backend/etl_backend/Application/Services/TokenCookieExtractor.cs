using etl_backend.Application.Abstraction;

namespace etl_backend.Application.Services;

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