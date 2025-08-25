using etl_backend.Application.Dtos;

namespace etl_backend.Application.KeycalokAuth.Abstraction;

public interface IKeycloakAdminClient
{
    Task<IEnumerable<UserWithRolesDto>> GetAllUsersAsync(CancellationToken cancellationToken);
    Task<UserWithRolesDto> GetUserByIdAsync(string userId, CancellationToken cancellationToken);
    Task<UserDto> CreateUserAsync(UserCreateDto newUser, CancellationToken cancellationToken);
    Task<UserDto> EditUserAsync(string userId, EditUserRequestDto userToUpdate, CancellationToken cancellationToken);
    Task DeleteUserAsync(string userId, CancellationToken cancellationToken);
    
    Task AddRolesToUserAsync(string userId, IEnumerable<RoleDto> roles, CancellationToken cancellationToken);
    Task RemoveRolesFromUserAsync(string userId, IEnumerable<RoleDto> roles, CancellationToken cancellationToken);
    
    Task<IEnumerable<RoleDto>> GetAllRolesAsync(CancellationToken cancellationToken);
    
}
