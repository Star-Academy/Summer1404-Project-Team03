using etl_backend.Services.Auth.keycloakAuthService.Dtos;

namespace etl_backend.Services.Auth.keycloakAuthService.Abstraction;

public interface IKeycloakLoginService
{
    string GenerateLoginUrl(string callbackUrl);
    Task<TokenResponseDto> ExchangeCodeForTokensAsync(string code, string redirectUri, CancellationToken ct = default);
}