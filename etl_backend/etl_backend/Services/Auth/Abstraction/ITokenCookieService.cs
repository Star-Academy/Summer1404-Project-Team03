using etl_backend.Services.Auth.keycloakService.Dtos;

namespace etl_backend.Services.Auth.Abstraction;

public interface ITokenCookieService
{
    void SetTokens(HttpResponse response, TokenResponseDto tokenResponse);
    void RemoveTokens(HttpResponse response);   
}