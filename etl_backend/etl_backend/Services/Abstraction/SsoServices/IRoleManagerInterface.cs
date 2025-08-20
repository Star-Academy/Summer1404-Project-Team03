namespace etl_backend.Services.Abstraction;

public interface IRoleManagerService
{
    Task<IEnumerable<string>> GetUserRolesAsync(string userId, string accessToken, CancellationToken cancellationToken);
    Task AssignRolesToUserAsync(string userId, IEnumerable<string> roles, string accessToken, CancellationToken cancellationToken);
    Task RemoveRolesFromUserAsync(string userId, IEnumerable<string> roles, string accessToken, CancellationToken cancellationToken);
}
