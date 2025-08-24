using System.Security.Claims;
using etl_backend.Services.Dtos;

namespace etl_backend.Services.Auth.Abstraction;

public interface IRoleExtractor
{
    Task<IEnumerable<RoleDto>> ExtractRoles(ClaimsPrincipal principal, string scope, string key);
}