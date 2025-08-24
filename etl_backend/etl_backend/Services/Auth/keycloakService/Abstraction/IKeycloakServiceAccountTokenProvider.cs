namespace etl_backend.Services.Auth.keycloakService.Abstraction;

public interface IKeycloakServiceAccountTokenProvider
{
    Task<string?> GetServiceAccountTokenAsync(CancellationToken ct = default);
}