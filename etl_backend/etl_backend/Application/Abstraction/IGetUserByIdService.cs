using etl_backend.Application.Dtos;

namespace etl_backend.Application.Abstraction;


public interface IGetUserByIdService
{
    Task<UserWithRolesDto> ExecuteAsync(string userId, CancellationToken cancellationToken);
}