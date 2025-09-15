using Application.Dtos;

namespace Application.Users.GetUserById.ServiceAbstractions;

public interface IGetUserByIdService
{
    Task<UserWithRolesDto> GetUserByIdAsync(string userId, CancellationToken cancellationToken);

}