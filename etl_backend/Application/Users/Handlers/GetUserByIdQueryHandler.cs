using Application.Common.Authorization;
using Application.ValueObjects;
using MediatR;

namespace Application.Users.Handlers;

[RequireRole(AppRoles.SysAdmin)]
public record GetUserByIdQuery(string UserId) : IRequest<UserWithRolesDto>;