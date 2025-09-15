using Application.Common.Authorization;
using Application.Plugins.Dtos;
using Domain.Enums;
using Domain.ValueObjects.PluginConfig;
using MediatR;

namespace Application.Plugins.UpdatePlugin;

[RequireRole(AppRoles.Analyst, AppRoles.DataAdmin, AppRoles.SysAdmin)]
public record UpdatePluginCommand(
    string WorkflowId,
    string PluginId,
    PluginType PluginType,
    Dictionary<string, object> Config 
) : IRequest<PluginDto>;