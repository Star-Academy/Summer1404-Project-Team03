namespace etl_backend.Services.Abstraction;

public interface IKeycloakAdminClient
{
    Task<IEnumerable<UserWithRolesDto>> GetAllUsersAsync(string accessToken, CancellationToken cancellationToken);
    Task<UserWithRolesDto> GetUserByIdAsync(string userId, string accessToken, CancellationToken cancellationToken);
    Task CreateUserAsync(UserDto newUser, IEnumerable<string> roles, string accessToken, CancellationToken cancellationToken);
    Task EditUserAsync(string userId, UserDto updatedUser, IEnumerable<string>? roles, string accessToken, CancellationToken cancellationToken);
    Task DeleteUserAsync(string userId, string accessToken, CancellationToken cancellationToken);
}
