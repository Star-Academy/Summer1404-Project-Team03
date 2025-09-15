using etl_backend.Application.Abstraction;
using etl_backend.Application.Dtos;
using etl_backend.Application.KeycalokAuth.Abstraction;

namespace etl_backend.Application.Services;

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