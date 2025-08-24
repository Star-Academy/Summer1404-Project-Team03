using etl_backend.Services.Dtos;

namespace etl_backend.Services.Abstraction.Admin;

public interface IEditUserRolesService
{
    Task<UserWithRolesDto> ExecuteAsync(
        string userId,
        IEnumerable<RoleDto> rolesToAdd,
        IEnumerable<RoleDto> rolesToRemove,
        CancellationToken cancellationToken);
}