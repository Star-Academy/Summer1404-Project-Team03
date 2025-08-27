using etl_backend.Application.KeycalokAuth.Abstraction;
using etl_backend.Application.UsersAuth.Abstraction;

namespace etl_backend.Application.UsersAuth.Services;

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