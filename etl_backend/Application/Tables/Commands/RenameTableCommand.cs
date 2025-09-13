using Application.Common.Authorization;
using MediatR;

namespace Application.Tables.Commands;
[RequireRole(AppRoles.DataAdmin, AppRoles.SysAdmin)]
public record RenameTableCommand(
    int SchemaId,
    string NewTableName
) : IRequest;
