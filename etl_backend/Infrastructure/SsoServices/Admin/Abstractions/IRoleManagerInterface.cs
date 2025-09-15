using Application.Dtos;
using Infrastructure.Dtos;

namespace Infrastructure.SsoServices.Admin.Abstractions;

public interface IRoleManagerService
{
    Task<IEnumerable<RoleDto>> GetUserRolesAsync(string userId, string accessToken, CancellationToken cancellationToken);

    Task AssignRolesToUserAsync(string userId, IEnumerable<RoleDto> roles, string accessToken,
        CancellationToken cancellationToken);

    Task RemoveRolesFromUserAsync(string userId, IEnumerable<RoleDto> roles, string accessToken,
        CancellationToken cancellationToken);
    Task<IEnumerable<RoleDto>> GetAllRolesAsync(string accessToken, CancellationToken cancellationToken);
}