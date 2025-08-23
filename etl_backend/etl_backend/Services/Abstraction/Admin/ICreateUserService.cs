using etl_backend.Services.Dtos;

namespace etl_backend.Services.Abstraction.Admin;

public interface ICreateUserService
{
    Task ExecuteAsync(UserDto newUser, IEnumerable<string> roles, string accessToken, CancellationToken cancellationToken);
}
