using etl_backend.Application.KeycalokAuth.Dtos;

namespace etl_backend.Application.KeycalokAuth.Abstraction;

public interface IAccessTokenRefreshable
{
    Task<TokenResponseDto?> RefreshAccessTokenAsync(string refreshToken, CancellationToken ct = default);
}