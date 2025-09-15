using etl_backend.Application.Abstraction;
using etl_backend.Application.KeycalokAuth.Abstraction;

namespace etl_backend.Application.Services;

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