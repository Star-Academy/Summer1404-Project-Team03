using Application.Common.Authorization;
using Application.Plugins.Dtos;
using Domain.Enums;
using Domain.ValueObjects.PluginConfig;
using MediatR;

namespace Application.Plugins.AddPlugin;

[RequireRole(AppRoles.Analyst, AppRoles.DataAdmin, AppRoles.SysAdmin)]
public record AddPluginCommand(
    string WorkflowId,
    PluginType PluginType, 
    object Config,
    int? Order = null
) : IRequest<PluginDto>;