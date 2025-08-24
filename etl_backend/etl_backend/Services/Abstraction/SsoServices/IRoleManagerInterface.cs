using etl_backend.Services.Dtos;

namespace etl_backend.Services.Abstraction.SsoServices;

public interface IRoleManagerService
{
    Task<IEnumerable<RoleDto>> GetUserRolesAsync(string userId, string accessToken, CancellationToken cancellationToken);

    Task AssignRolesToUserAsync(string userId, IEnumerable<RoleDto> roles, string accessToken,
        CancellationToken cancellationToken);

    Task RemoveRolesFromUserAsync(string userId, IEnumerable<RoleDto> roles, string accessToken,
        CancellationToken cancellationToken);
    Task<IEnumerable<RoleDto>> GetAllRolesAsync(string accessToken, CancellationToken cancellationToken);
}