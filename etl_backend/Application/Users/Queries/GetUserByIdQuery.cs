using Application.Common.Authorization;
using Application.Dtos;
using MediatR;

namespace Application.Users.Queries;

[RequireRole(AppRoles.SysAdmin)]
public record GetUserByIdQuery(string UserId) : IRequest<UserWithRolesDto>;