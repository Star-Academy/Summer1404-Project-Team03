using Application.Common.Authorization;
using MediatR;

namespace Application.Tables.Queries;
[RequireRole(AppRoles.DataAdmin, AppRoles.SysAdmin)]
public record ListColumnsQuery(int SchemaId) : IRequest<List<ColumnListItem>>;

public record ColumnListItem(
    int Id,
    int OrdinalPosition,
    string Name,
    string Type,
    string? OriginalName
);