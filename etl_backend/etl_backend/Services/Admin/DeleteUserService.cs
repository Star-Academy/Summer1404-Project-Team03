using etl_backend.Services.Abstraction.Admin;
using etl_backend.Services.Abstraction.SsoServices;

namespace etl_backend.Services.Admin;

public class DeleteUserService : IDeleteUserService
{
    private readonly IKeycloakAdminClient _keycloakAdminClient;

    public DeleteUserService(IKeycloakAdminClient keycloakAdminClient)
    {
        _keycloakAdminClient = keycloakAdminClient;
    }

    public async Task ExecuteAsync(string userId, CancellationToken cancellationToken)
    {
        await _keycloakAdminClient.DeleteUserAsync(userId, cancellationToken);
    }
}