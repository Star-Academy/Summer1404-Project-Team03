using etl_backend.Services.Abstraction.Admin;
using etl_backend.Services.Abstraction.SsoServices;
using etl_backend.Services.Dtos;

namespace etl_backend.Services.Admin;

public class GetUserByIdService : IGetUserByIdService
{
    private readonly IKeycloakAdminClient _keycloakAdminClient;

    public GetUserByIdService(IKeycloakAdminClient keycloakAdminClient)
    {
        _keycloakAdminClient = keycloakAdminClient;
    }

    public async Task<UserWithRolesDto> ExecuteAsync(string userId, string accessToken, CancellationToken cancellationToken)
    {
        return await _keycloakAdminClient.GetUserByIdAsync(userId, accessToken, cancellationToken);
    }
}