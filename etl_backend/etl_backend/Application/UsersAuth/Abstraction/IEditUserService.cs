using etl_backend.Application.UsersAuth.Dtos;

namespace etl_backend.Application.UsersAuth.Abstraction;

public interface IEditUserService
{
    Task ExecuteAsync(string userId, EditUserRequestDto userToUpdate, CancellationToken cancellationToken);
}

