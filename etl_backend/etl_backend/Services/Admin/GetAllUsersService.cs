using System.Net.Http.Headers;
using etl_backend.Services.Abstraction;
using etl_backend.Services.Abstraction.Admin;
using etl_backend.Services.Abstraction.SsoServices;
using etl_backend.Services.Dtos;

namespace etl_backend.Services.Admin;

public class GetAllUsersService : IGetAllUsersService
{
    private readonly IKeycloakAdminClient _keycloakAdminClient;

    public GetAllUsersService(IKeycloakAdminClient keycloakAdminClient)
    {
        _keycloakAdminClient = keycloakAdminClient;
    }

    public async Task<IEnumerable<UserWithRolesDto>> ExecuteAsync(string accessToken, CancellationToken cancellationToken)
    {
        return await _keycloakAdminClient.GetAllUsersAsync(accessToken, cancellationToken);
    }
}