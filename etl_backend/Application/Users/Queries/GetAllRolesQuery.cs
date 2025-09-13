using Application.Common.Authorization;
using Application.Dtos;
using MediatR;

namespace Application.Users.Queries;

[RequireRole(AppRoles.SysAdmin, AppRoles.DataAdmin, AppRoles.Analyst)]
public record GetAllRolesQuery : IRequest<List<RoleDto>>;