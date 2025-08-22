using etl_backend.Services.Auth.keycloakAuthService.Dtos;

namespace etl_backend.Services.Auth.keycloakService.Abstraction;

public interface IParseTokenResponse
{
    TokenResponseDto ParseTokenResponse(string json);
}