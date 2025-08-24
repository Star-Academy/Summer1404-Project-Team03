using etl_backend.Services.Abstraction.Admin;
using etl_backend.Services.Abstraction.SsoServices;
using etl_backend.Services.Dtos;

namespace etl_backend.Services.Admin;

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