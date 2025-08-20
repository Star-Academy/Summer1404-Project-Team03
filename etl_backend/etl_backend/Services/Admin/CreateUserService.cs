using etl_backend.Services.Abstraction;
using etl_backend.Services.Abstraction.Admin;

namespace etl_backend.Services.Admin;

public class CreateUserService : ICreateUserService
{
    private readonly IKeycloakAdminClient _keycloakAdminClient;

    public CreateUserService(IKeycloakAdminClient keycloakAdminClient)
    {
        _keycloakAdminClient = keycloakAdminClient;
    }

    public async Task ExecuteAsync(UserDto newUser, IEnumerable<string> roles, string accessToken, CancellationToken cancellationToken)
    {
        await _keycloakAdminClient.CreateUserAsync(newUser, roles, accessToken, cancellationToken);
    }
}