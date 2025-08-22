using etl_backend.Services.Auth.keycloakAuthService.Dtos;

namespace etl_backend.Services.Auth.keycloakService.Abstraction;

public interface IAccessTokenRefreshable
{
    Task<TokenResponseDto?> RefreshAccessTokenAsync(string refreshToken, CancellationToken ct = default);
}