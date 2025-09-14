using Application.Common.Authorization;
using MediatR;

namespace Application.Tables.Commands;
[RequireRole(AppRoles.DataAdmin, AppRoles.SysAdmin)]
public record DeleteTableCommand(int SchemaId) : IRequest;