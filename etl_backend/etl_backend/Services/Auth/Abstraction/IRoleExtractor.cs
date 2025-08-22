using System.Security.Claims;

namespace etl_backend.Services.Auth.Abstraction;

public interface IRoleExtractor
{
    IEnumerable<string> ExtractRoles(ClaimsPrincipal principal, string scope, string key);
}