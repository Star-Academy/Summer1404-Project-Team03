using etl_backend.Services.Dtos;

namespace etl_backend.Services.Abstraction.Admin;

public interface IEditUserService
{
    Task ExecuteAsync(string userId, EditUserRequestDto userToUpdate, CancellationToken cancellationToken);
}

