using Infrastructure.Dtos;

namespace Infrastructure.SsoServices.User.Abstractions;

public interface IKeycloakAuthService
{
    string GenerateLoginUrl(string callbackUrl);
    Task<TokenResponseDto> ExchangeCodeForTokensAsync(string code, string redirectUri, CancellationToken ct = default);
    public string GenerateChangePasswordUrlPage();
}