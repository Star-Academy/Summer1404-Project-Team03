using Infrastructure.Dtos;

namespace Infrastructure.Identity.Abstractions;

public interface IEditUserService
{
    Task ExecuteAsync(string userId, EditUserRequestDto userToUpdate, CancellationToken cancellationToken);
}

