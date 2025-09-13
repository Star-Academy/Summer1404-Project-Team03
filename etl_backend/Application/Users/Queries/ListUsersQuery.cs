using Application.Common.Authorization;
using Application.ValueObjects;
using MediatR;

namespace Application.Users.Queries;

[RequireRole(AppRoles.SysAdmin)]
public record ListUsersQuery : IRequest<List<UserWithRolesDto>>;
