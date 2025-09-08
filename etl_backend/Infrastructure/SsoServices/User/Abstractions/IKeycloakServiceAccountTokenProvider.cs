namespace Infrastructure.SsoServices.User.Abstractions;

public interface IKeycloakServiceAccountTokenProvider
{
    Task<string?> GetServiceAccountTokenAsync(CancellationToken ct = default);
}