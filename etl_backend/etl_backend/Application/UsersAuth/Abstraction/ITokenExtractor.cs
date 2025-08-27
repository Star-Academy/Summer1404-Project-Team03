namespace etl_backend.Application.UsersAuth.Abstraction;

public interface ITokenExtractor
{
    string? GetAccessToken(HttpRequest request, string accessCookieName);
    string? GetRefreshToken(HttpRequest request, string refreshCookieName);
}