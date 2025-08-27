using etl_backend.Application.KeycalokAuth.Abstraction;
using etl_backend.Application.UsersAuth.Abstraction;
using etl_backend.Application.UsersAuth.Dtos;

namespace etl_backend.Application.UsersAuth.Services;

public class GetAllRolesService: IGetRolesList
{
    private readonly IKeycloakAdminClient _adminClient;

    public GetAllRolesService(IKeycloakAdminClient adminClient)
    {
        _adminClient = adminClient;
    }

    public async Task<IEnumerable<RoleDto>> ExecuteAsync(CancellationToken cancellationToken)
    {
        return await _adminClient.GetAllRolesAsync(cancellationToken);
    }
}