using Application.Common.Authorization;
using MediatR;

namespace Application.Users.Commands;

[RequireRole(AppRoles.SysAdmin)]
public record DeleteUserCommand(string UserId) : IRequest;