using Application.Common.Authorization;
using MediatR;
namespace Application.Plugins.ReorderPlugins;

[RequireRole(AppRoles.Analyst, AppRoles.DataAdmin, AppRoles.SysAdmin)]
public record ReorderPluginsCommand(
    string WorkflowId,
    List<string> PluginIdsInOrder 
) : IRequest;