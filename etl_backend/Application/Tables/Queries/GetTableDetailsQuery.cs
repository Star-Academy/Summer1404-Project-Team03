using Application.Common.Authorization;
using Application.ValueObjects;
using MediatR;

namespace Application.Tables.Queries;
[RequireRole(AppRoles.Analyst, AppRoles.DataAdmin, AppRoles.SysAdmin)]
public record GetTableDetailsQuery(int SchemaId) : IRequest<TableDetailsDto>;