using Microsoft.AspNetCore.Http;

namespace Infrastructure.Identity.Abstractions;

public interface ITokenExtractor
{
    string? GetAccessToken(HttpRequest request, string accessCookieName);
    string? GetRefreshToken(HttpRequest request, string refreshCookieName);
}