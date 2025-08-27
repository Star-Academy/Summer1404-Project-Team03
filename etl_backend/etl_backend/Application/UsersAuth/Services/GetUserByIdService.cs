using etl_backend.Application.KeycalokAuth.Abstraction;
using etl_backend.Application.UsersAuth.Abstraction;
using etl_backend.Application.UsersAuth.Dtos;

namespace etl_backend.Application.UsersAuth.Services;

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