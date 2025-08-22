namespace etl_backend.Services.Auth.keycloakService.Abstraction;

public interface IKeycloakRefreshTokenRevokable
{
    Task<bool> RevokeTokenAsynk(string refreshToken, CancellationToken ct = default);
}