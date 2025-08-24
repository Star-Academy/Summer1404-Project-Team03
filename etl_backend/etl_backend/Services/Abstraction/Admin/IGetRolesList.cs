using etl_backend.Services.Dtos;

namespace etl_backend.Services.Abstraction.Admin;

public interface IGetRolesList
{
    Task<IEnumerable<RoleDto>> ExecuteAsync(CancellationToken cancellationToken);
}