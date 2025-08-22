namespace etl_backend.Services.Auth.keycloakService.Abstraction;

public interface IKeycloakTokenHandler
{
    Task<string?> GetOrRefreshAccessTokenAsync(HttpContext context, CancellationToken cancellationToken);
}

