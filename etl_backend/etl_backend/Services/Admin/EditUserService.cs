using etl_backend.Services.Abstraction;
using etl_backend.Services.Abstraction.Admin;

namespace etl_backend.Services.Admin;

public class EditUserService : IEditUserService
{
    private readonly IKeycloakAdminClient _keycloakAdminClient;

    public EditUserService(IKeycloakAdminClient keycloakAdminClient)
    {
        _keycloakAdminClient = keycloakAdminClient;
    }

    public async Task ExecuteAsync(string userId, UserDto updatedUser, IEnumerable<string>? roles, string accessToken, CancellationToken cancellationToken)
    {
        await _keycloakAdminClient.EditUserAsync(userId, updatedUser, roles, accessToken, cancellationToken);
    }
}