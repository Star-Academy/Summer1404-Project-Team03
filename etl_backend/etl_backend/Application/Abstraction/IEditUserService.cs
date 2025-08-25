using etl_backend.Application.Dtos;

namespace etl_backend.Application.Abstraction;

public interface IEditUserService
{
    Task ExecuteAsync(string userId, EditUserRequestDto userToUpdate, CancellationToken cancellationToken);
}

