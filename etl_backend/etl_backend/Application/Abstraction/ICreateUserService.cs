using etl_backend.Application.Dtos;

namespace etl_backend.Application.Abstraction;

public interface ICreateUserService
{
    Task<UserDto> ExecuteAsync(UserCreateDto newUser, CancellationToken cancellationToken);
}
