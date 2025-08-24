using etl_backend.Application.Abstraction;
using etl_backend.Application.Dtos;
using etl_backend.Application.KeycalokAuth.Abstraction;

namespace etl_backend.Application.Services;

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