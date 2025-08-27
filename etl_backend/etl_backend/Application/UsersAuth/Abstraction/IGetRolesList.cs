using etl_backend.Application.UsersAuth.Dtos;

namespace etl_backend.Application.UsersAuth.Abstraction;

public interface IGetRolesList
{
    Task<IEnumerable<RoleDto>> ExecuteAsync(CancellationToken cancellationToken);
}