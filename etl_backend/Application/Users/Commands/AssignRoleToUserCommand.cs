using Application.Common.Authorization;
using MediatR;

namespace Application.Users.Commands;

[RequireRole(AppRoles.SysAdmin)]
public record AssignRoleToUserCommand(
    string UserId,
    string RoleName
) : IRequest;