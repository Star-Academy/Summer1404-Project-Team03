using Microsoft.AspNetCore.Http;

namespace Infrastructure.SsoServices.User.Abstractions;

public interface IKeycloakTokenHandler
{
    Task<string?> GetOrRefreshAccessTokenAsync(HttpContext context, CancellationToken cancellationToken);
}

