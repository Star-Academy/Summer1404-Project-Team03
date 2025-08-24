namespace etl_backend.Application.KeycalokAuth.Abstraction;

public interface IKeycloakRefreshTokenRevokable
{
    Task<bool> RevokeTokenAsynk(string refreshToken, CancellationToken ct = default);
}