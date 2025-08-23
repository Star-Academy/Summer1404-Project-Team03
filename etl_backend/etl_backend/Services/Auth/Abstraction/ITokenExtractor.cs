namespace etl_backend.Services.Auth.Abstraction;

public interface ITokenExtractor
{
    string? GetAccessToken(HttpRequest request, string accessCookieName);
    string? GetRefreshToken(HttpRequest request, string refreshCookieName);
}