using etl_backend.Application.Abstraction;
using etl_backend.Application.Dtos;
using etl_backend.Application.KeycalokAuth.Abstraction;

namespace etl_backend.Application.Services;

public class GetUserByIdService : IGetUserByIdService
{
    private readonly IKeycloakAdminClient _keycloakAdminClient;

    public GetUserByIdService(IKeycloakAdminClient keycloakAdminClient)
    {
        _keycloakAdminClient = keycloakAdminClient;
    }

    public async Task<UserWithRolesDto> ExecuteAsync(string userId, CancellationToken cancellationToken)
    {
        return await _keycloakAdminClient.GetUserByIdAsync(userId, cancellationToken);
    }
}