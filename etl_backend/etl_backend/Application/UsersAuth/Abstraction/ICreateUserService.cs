using etl_backend.Application.UsersAuth.Dtos;

namespace etl_backend.Application.UsersAuth.Abstraction;

public interface ICreateUserService
{
    Task<UserDto> ExecuteAsync(UserCreateDto newUser, CancellationToken cancellationToken);
}
