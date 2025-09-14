using Application.Dtos;

namespace Application.Users.CreateUser.ServiceAbstractions;

public interface ICreateUser
{
    Task<UserDto> CreateUserAsync(UserCreateDto newUser, CancellationToken cancellationToken);
}