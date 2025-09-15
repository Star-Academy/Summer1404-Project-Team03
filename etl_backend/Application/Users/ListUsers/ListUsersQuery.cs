using Application.Common.Authorization;
using Application.Dtos;
using MediatR;

namespace Application.Users.Queries;

[RequireRole(AppRoles.SysAdmin)]
public record ListUsersQuery : IRequest<List<UserWithRolesDto>>;
