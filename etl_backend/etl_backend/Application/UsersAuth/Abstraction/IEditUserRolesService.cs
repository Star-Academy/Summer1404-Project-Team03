using etl_backend.Application.UsersAuth.Dtos;

namespace etl_backend.Application.UsersAuth.Abstraction;

public interface IEditUserRolesService
{
    Task<UserWithRolesDto> ExecuteAsync(
        string userId,
        IEnumerable<RoleDto> rolesToAdd,
        IEnumerable<RoleDto> rolesToRemove,
        CancellationToken cancellationToken);
}