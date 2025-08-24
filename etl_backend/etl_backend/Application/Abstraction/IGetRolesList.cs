using etl_backend.Application.Dtos;

namespace etl_backend.Application.Abstraction;

public interface IGetRolesList
{
    Task<IEnumerable<RoleDto>> ExecuteAsync(CancellationToken cancellationToken);
}