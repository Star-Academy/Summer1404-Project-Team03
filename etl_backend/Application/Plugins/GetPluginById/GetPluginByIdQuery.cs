using Application.Common.Authorization;
using Application.Plugins.Dtos;
using MediatR;

namespace Application.Plugins.GetPluginById;

[RequireRole(AppRoles.Analyst, AppRoles.DataAdmin, AppRoles.SysAdmin)]
public record GetPluginByIdQuery(
    string WorkflowId,
    string PluginId
) : IRequest<PluginDto>;