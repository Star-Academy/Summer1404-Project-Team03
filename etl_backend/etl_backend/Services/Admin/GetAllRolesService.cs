using etl_backend.Services.Abstraction.Admin;
using etl_backend.Services.Abstraction.SsoServices;
using etl_backend.Services.Dtos;

namespace etl_backend.Services.Admin;

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