using etl_backend.Application.UsersAuth.Dtos;

namespace etl_backend.Application.UsersAuth.Abstraction;
public interface IGetAllUsersService
{
    Task<IEnumerable<UserWithRolesDto>> ExecuteAsync(CancellationToken cancellationToken);
}