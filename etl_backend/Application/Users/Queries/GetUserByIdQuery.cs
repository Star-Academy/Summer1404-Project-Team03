using Application.Common.Authorization;
using Application.ValueObjects;
using MediatR;

namespace Application.Users.Queries;

[RequireRole(AppRoles.SysAdmin)]
public record GetUserByIdQuery(string UserId) : IRequest<UserWithRolesDto>;