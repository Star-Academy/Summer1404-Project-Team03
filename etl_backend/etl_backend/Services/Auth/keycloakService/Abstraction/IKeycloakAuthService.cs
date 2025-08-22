using etl_backend.Services.Auth.keycloakAuthService.Dtos;

namespace etl_backend.Services.Auth.keycloakService.Abstraction;

public interface IKeycloakAuthService
{
    string GenerateLoginUrl(string callbackUrl);
    Task<TokenResponseDto> ExchangeCodeForTokensAsync(string code, string redirectUri, CancellationToken ct = default);
}