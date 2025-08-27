using etl_backend.Application.KeycalokAuth.Abstraction;
using etl_backend.Application.UsersAuth.Abstraction;
using etl_backend.Application.UsersAuth.Dtos;

namespace etl_backend.Application.UsersAuth.Services;

public class EditUserRolesService : IEditUserRolesService
{
    private readonly IKeycloakAdminClient _adminClient;

    public EditUserRolesService(IKeycloakAdminClient adminClient)
    {
        _adminClient = adminClient;
    }

    public async Task<UserWithRolesDto> ExecuteAsync(
        string userId,
        IEnumerable<RoleDto> rolesToAdd,
        IEnumerable<RoleDto> rolesToRemove,
        CancellationToken cancellationToken)
    {
        if (rolesToAdd != null && rolesToAdd.Any())
        {
            await _adminClient.AddRolesToUserAsync(userId, rolesToAdd, cancellationToken);
        }

        if (rolesToRemove != null && rolesToRemove.Any())
        {
            await _adminClient.RemoveRolesFromUserAsync(userId, rolesToRemove, cancellationToken);
        }

        var updatedUser = await _adminClient.GetUserByIdAsync(userId, cancellationToken);
        return updatedUser;
    }
}