using Application.Dtos;

namespace Application.Services.Abstractions;

public interface IUserRoleManagementService
{
    Task AddRolesToUserAsync(string userId, RoleDto[] roles, CancellationToken cancellationToken);
    Task RemoveRolesFromUserAsync(string userId, IEnumerable<RoleDto> roles, CancellationToken cancellationToken);
    
}
