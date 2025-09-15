using etl_backend.Application.Abstraction;
using etl_backend.Application.Dtos;
using etl_backend.Application.KeycalokAuth.Abstraction;

namespace etl_backend.Application.Services;

public class EditUserService : IEditUserService
{
    private readonly IKeycloakAdminClient _keycloakAdminClient;

    public EditUserService(IKeycloakAdminClient keycloakAdminClient)
    {
        _keycloakAdminClient = keycloakAdminClient;
    }

    public async Task ExecuteAsync(string userId, EditUserRequestDto userToUpdate,
        CancellationToken cancellationToken)
    {
        await _keycloakAdminClient.EditUserAsync(userId, userToUpdate, cancellationToken);
    }
}