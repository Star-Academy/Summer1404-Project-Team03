namespace etl_backend.Application.Abstraction;

public interface ITokenExtractor
{
    string? GetAccessToken(HttpRequest request, string accessCookieName);
    string? GetRefreshToken(HttpRequest request, string refreshCookieName);
}