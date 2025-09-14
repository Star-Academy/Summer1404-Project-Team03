using Application.Common.Authorization;
using MediatR;

namespace Application.Tables.Queries;
[RequireRole(AppRoles.Analyst, AppRoles.DataAdmin, AppRoles.SysAdmin)]
public record ListTablesQuery : IRequest<List<TableListItem>>;

public record TableListItem(
    int SchemaId,
    string TableName,
    string OriginalFileName,
    int ColumnCount
);