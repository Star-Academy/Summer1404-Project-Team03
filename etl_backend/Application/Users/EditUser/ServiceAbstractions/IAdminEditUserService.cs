using Application.Dtos;

namespace Application.Users.EditUser.ServiceAbstractions;

public interface IAdminEditUserService
{
    Task<UserDto> EditUserAsync(string userId, EditUserRequestDto userToUpdate, CancellationToken cancellationToken);
}