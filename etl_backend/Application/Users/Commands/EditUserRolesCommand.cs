using Application.Common.Authorization;
using Application.ValueObjects;
using MediatR;

namespace Application.Users.Commands;

[RequireRole(AppRoles.SysAdmin)]
public record EditUserRolesCommand(
    string UserId,
    List<RoleDto> RolesToAdd,
    List<RoleDto> RolesToRemove
) : IRequest;