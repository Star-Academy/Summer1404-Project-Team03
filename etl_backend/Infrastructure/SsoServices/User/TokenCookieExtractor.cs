using Infrastructure.Identity.Abstractions;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.SsoServices.User;

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