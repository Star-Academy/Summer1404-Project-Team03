using etl_backend.Application.UsersAuth.Dtos;

namespace etl_backend.Application.UsersAuth.Abstraction;


public interface IGetUserByIdService
{
    Task<UserWithRolesDto> ExecuteAsync(string userId, CancellationToken cancellationToken);
}