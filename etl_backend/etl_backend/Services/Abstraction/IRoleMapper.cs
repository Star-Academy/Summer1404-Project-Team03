using System.Security.Claims;

namespace etl_backend.Services.Abstraction;

public interface IRoleMapper
{
    Task MapRolesAsync(ClaimsPrincipal principal, string? rawToken = null, CancellationToken ct = default);
}
