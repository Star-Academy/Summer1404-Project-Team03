using etl_backend.Application.Dtos;

namespace etl_backend.Application.Abstraction;
public interface IGetAllUsersService
{
    Task<IEnumerable<UserWithRolesDto>> ExecuteAsync(CancellationToken cancellationToken);
}