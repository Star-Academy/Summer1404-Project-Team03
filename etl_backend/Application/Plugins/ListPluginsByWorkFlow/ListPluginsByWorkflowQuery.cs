using Application.Common.Authorization;
using Application.Plugins.Dtos;
using MediatR;

namespace Application.Plugins.ListPluginsByWorkFlow;

[RequireRole(AppRoles.Analyst, AppRoles.DataAdmin, AppRoles.SysAdmin)]
public record ListPluginsByWorkflowQuery(string WorkflowId) : IRequest<List<PluginDto>>;