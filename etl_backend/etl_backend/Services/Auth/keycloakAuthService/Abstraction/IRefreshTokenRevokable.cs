namespace etl_backend.Services.Auth.keycloakAuthService.Abstraction;

public interface IRefreshTokenRevokable
{
    Task<bool> RevokeTokenAsynk(string refreshToken, CancellationToken ct = default);
}