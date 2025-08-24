using etl_backend.Application.KeycalokAuth.Dtos;

namespace etl_backend.Application.KeycalokAuth.Abstraction;

public interface IKeycloakAuthService
{
    string GenerateLoginUrl(string callbackUrl);
    Task<TokenResponseDto> ExchangeCodeForTokensAsync(string code, string redirectUri, CancellationToken ct = default);
    public string GenerateChangePasswordUrl();
}