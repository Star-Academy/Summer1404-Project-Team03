using Application.Dtos;

namespace Application.Users.GetAllRoles.ServiceAbstractions;

public interface IGetAllRoles
{
    Task<IEnumerable<RoleDto>> GetAllRolesAsync(CancellationToken cancellationToken);
}