using Application.Common.Authorization;
using Application.WorkFlow.Dtos;
using MediatR;

namespace Application.WorkFlow.ListWorkFlows;

[RequireRole(AppRoles.Analyst, AppRoles.DataAdmin, AppRoles.SysAdmin)]
public record ListWorkflowsQuery : IRequest<List<WorkflowDto>>;