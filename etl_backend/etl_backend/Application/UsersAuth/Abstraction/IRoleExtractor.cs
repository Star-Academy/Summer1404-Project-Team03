using System.Security.Claims;
using etl_backend.Application.UsersAuth.Dtos;

namespace etl_backend.Application.UsersAuth.Abstraction;

public interface IRoleExtractor
{
    Task<IEnumerable<RoleDto>> ExtractRoles(ClaimsPrincipal principal, string scope, string key);
}