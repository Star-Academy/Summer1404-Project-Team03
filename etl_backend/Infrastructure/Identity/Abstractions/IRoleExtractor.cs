using System.Security.Claims;
using Infrastructure.Dtos;

namespace Infrastructure.Identity.Abstractions;

public interface IRoleExtractor
{
    Task<IEnumerable<RoleDto>> ExtractRoles(ClaimsPrincipal principal, string scope, string key);
}