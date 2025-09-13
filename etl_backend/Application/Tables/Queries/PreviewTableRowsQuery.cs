using Application.Common.Authorization;
using Application.ValueObjects;
using MediatR;

namespace Application.Tables.Queries;
[RequireRole(AppRoles.Analyst, AppRoles.DataAdmin, AppRoles.SysAdmin)]
public record PreviewTableRowsQuery(
    int SchemaId,
    int Offset = 0,
    int Limit = 50,
    string? OrderBy = null,
    string? Direction = null
) : IRequest<RowPreviewDto>;