using System.Security.Claims;
using etl_backend.Application.Dtos;

namespace etl_backend.Application.Abstraction;

public interface IRoleExtractor
{
    Task<IEnumerable<RoleDto>> ExtractRoles(ClaimsPrincipal user, string scope, string key);
}