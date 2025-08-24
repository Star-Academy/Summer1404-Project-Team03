using etl_backend.Services.Dtos;

namespace etl_backend.Services.Abstraction.Admin;

public interface ICreateUserService
{
    Task<UserDto> ExecuteAsync(UserCreateDto newUser, CancellationToken cancellationToken);
}
