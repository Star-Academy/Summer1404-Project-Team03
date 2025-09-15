using etl_backend.Application.Dtos;

namespace etl_backend.Application.Abstraction;

public interface IEditUserRolesService
{
    Task<UserWithRolesDto> ExecuteAsync(
        string userId,
        IEnumerable<RoleDto> rolesToAdd,
        IEnumerable<RoleDto> rolesToRemove,
        CancellationToken cancellationToken);
}