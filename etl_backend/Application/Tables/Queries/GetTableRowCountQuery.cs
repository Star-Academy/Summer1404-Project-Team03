using Application.Common.Authorization;
using Application.ValueObjects;
using MediatR;

namespace Application.Tables.Queries;
[RequireRole(AppRoles.Analyst, AppRoles.DataAdmin, AppRoles.SysAdmin)]
public record GetTableRowCountQuery(int SchemaId, bool Exact = false) : IRequest<RowCountDto>;