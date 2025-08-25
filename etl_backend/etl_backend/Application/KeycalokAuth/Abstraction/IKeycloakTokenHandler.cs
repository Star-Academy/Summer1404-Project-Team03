namespace etl_backend.Application.KeycalokAuth.Abstraction;

public interface IKeycloakTokenHandler
{
    Task<string?> GetOrRefreshAccessTokenAsync(HttpContext context, CancellationToken cancellationToken);
}

