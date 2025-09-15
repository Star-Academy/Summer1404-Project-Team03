using Infrastructure.Dtos;

namespace Infrastructure.SsoServices.User.Abstractions;

public interface IAccessTokenRefreshable
{
    Task<TokenResponseDto?> RefreshAccessTokenAsync(string refreshToken, CancellationToken ct = default);
}