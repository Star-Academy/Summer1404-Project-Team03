using etl_backend.Application.KeycalokAuth.Dtos;

namespace etl_backend.Application.Abstraction;

public interface ITokenCookieService
{
    void SetTokens(HttpResponse response, TokenResponseDto tokenResponse);
    void RemoveTokens(HttpResponse response);   
}