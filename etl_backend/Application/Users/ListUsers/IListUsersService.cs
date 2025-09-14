using Application.Dtos;

namespace Application.Users.ListUsers;

public interface IListUsersService
{
    Task<IEnumerable<UserWithRolesDto>> GetAllUsersAsync(CancellationToken cancellationToken);
}