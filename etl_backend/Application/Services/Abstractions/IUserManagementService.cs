using Application.Dtos;

namespace Application.Services.Abstractions;

public interface IUserManagementService
{
    Task<IEnumerable<UserWithRolesDto>> GetAllUsersAsync(CancellationToken cancellationToken);
    Task<UserWithRolesDto> GetUserByIdAsync(string userId, CancellationToken cancellationToken);
    Task<UserDto> CreateUserAsync(UserCreateDto newUser, CancellationToken cancellationToken);
    Task<UserDto> EditUserAsync(string userId, EditUserRequestDto userToUpdate, CancellationToken cancellationToken);
    Task DeleteUserAsync(string userId, CancellationToken cancellationToken);
    
    Task AddRolesToUserAsync(string userId, RoleDto[] roles, CancellationToken cancellationToken);
    Task RemoveRolesFromUserAsync(string userId, IEnumerable<RoleDto> roles, CancellationToken cancellationToken);
    
    Task<IEnumerable<RoleDto>> GetAllRolesAsync(CancellationToken cancellationToken);
    
}
