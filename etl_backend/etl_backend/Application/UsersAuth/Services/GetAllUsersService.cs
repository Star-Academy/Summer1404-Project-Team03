using etl_backend.Application.KeycalokAuth.Abstraction;
using etl_backend.Application.UsersAuth.Abstraction;
using etl_backend.Application.UsersAuth.Dtos;

namespace etl_backend.Application.UsersAuth.Services;

public class GetAllUsersService : IGetAllUsersService
{
    private readonly IKeycloakAdminClient _keycloakAdminClient;

    public GetAllUsersService(IKeycloakAdminClient keycloakAdminClient)
    {
        _keycloakAdminClient = keycloakAdminClient;
    }

    public async Task<IEnumerable<UserWithRolesDto>> ExecuteAsync(CancellationToken cancellationToken)
    {
        return await _keycloakAdminClient.GetAllUsersAsync(cancellationToken);
    }
}