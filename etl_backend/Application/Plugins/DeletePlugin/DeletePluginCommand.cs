using Application.Common.Authorization;
using MediatR;

namespace Application.Plugins.DeletePlugin;

[RequireRole(AppRoles.Analyst, AppRoles.DataAdmin, AppRoles.SysAdmin)]
public record DeletePluginCommand(
    string WorkflowId,
    string PluginId
) : IRequest;