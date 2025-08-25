namespace etl_backend.Application.KeycalokAuth.Abstraction;

public interface IKeycloakServiceAccountTokenProvider
{
    Task<string?> GetServiceAccountTokenAsync(CancellationToken ct = default);
}