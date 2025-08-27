using etl_backend.Application.KeycalokAuth.Abstraction;
using etl_backend.Application.UsersAuth.Abstraction;
using etl_backend.Application.UsersAuth.Dtos;

namespace etl_backend.Application.UsersAuth.Services;

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