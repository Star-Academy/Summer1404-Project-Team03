using etl_backend.Services.Dtos;

namespace etl_backend.Services.Abstraction.Admin;
public interface IGetAllUsersService
{
    Task<IEnumerable<UserWithRolesDto>> ExecuteAsync(string accessToken, CancellationToken cancellationToken);
}