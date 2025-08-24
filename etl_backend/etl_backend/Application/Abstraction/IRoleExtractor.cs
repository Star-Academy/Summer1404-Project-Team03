using System.Security.Claims;
using etl_backend.Application.Dtos;

namespace etl_backend.Application.Abstraction;

public interface IRoleExtractor
{
    Task<IEnumerable<RoleDto>> ExtractRoles(ClaimsPrincipal principal, string scope, string key);
}