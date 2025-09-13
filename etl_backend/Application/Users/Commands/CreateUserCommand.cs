using Application.Common.Authorization;
using Application.ValueObjects;
using MediatR;

namespace Application.Users.Commands;

[RequireRole(AppRoles.SysAdmin)]
public record CreateUserCommand(
    string Username,
    string? Email = null,
    string? FirstName = null,
    string? LastName = null,
    string? Password = null
) : IRequest<UserDto>;