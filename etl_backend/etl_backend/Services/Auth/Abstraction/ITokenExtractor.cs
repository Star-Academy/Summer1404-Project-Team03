namespace etl_backend.Services.Auth.Abstraction;

public interface ITokenExtractor
{
    string? GetAccessToken(HttpRequest request, string AccessCookieName);
    string? GetRefreshToken(HttpRequest request, string refreshCookieName);
}