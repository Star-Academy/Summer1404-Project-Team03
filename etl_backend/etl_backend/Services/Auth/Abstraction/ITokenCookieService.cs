using etl_backend.Services.Auth.keycloakAuthService.Dtos;

namespace etl_backend.Services.Auth.Abstraction;

public interface ITokenCookieService
{
    void SetTokens(HttpResponse response, TokenResponseDto tokenResponse);
    void DeleteTokens(HttpResponse response);   
}