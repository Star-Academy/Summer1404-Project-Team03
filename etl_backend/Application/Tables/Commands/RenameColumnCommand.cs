using Application.Common.Authorization;
using MediatR;

namespace Application.Tables.Commands;
[RequireRole(AppRoles.DataAdmin, AppRoles.SysAdmin)]
public record RenameColumnCommand(
    int SchemaId,
    int ColumnId,
    string NewName
) : IRequest;