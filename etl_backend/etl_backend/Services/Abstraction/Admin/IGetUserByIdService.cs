using etl_backend.Services.Dtos;

namespace etl_backend.Services.Abstraction.Admin;


public interface IGetUserByIdService
{
    Task<UserWithRolesDto> ExecuteAsync(string userId, CancellationToken cancellationToken);
}