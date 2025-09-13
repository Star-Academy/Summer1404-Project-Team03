using Application.Common.Authorization;
using Application.ValueObjects;
using MediatR;

namespace Application.Users.Commands;

[RequireRole(AppRoles.SysAdmin, AppRoles.Analyst, AppRoles.DataAdmin)]
public record EditUserCommand(
    string UserId,
    string? Username = null,
    string? Email = null,
    string? FirstName = null,
    string? LastName = null
) : IRequest<UserDto>;