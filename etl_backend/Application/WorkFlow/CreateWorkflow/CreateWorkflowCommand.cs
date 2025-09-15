using Application.Common.Authorization;
using Application.WorkFlow.Dtos;
using MediatR;

namespace Application.WorkFlow.CreateWorkflow;

[RequireRole(AppRoles.Analyst, AppRoles.DataAdmin, AppRoles.SysAdmin)]
public record CreateWorkflowCommand(
    string Name,
    string? Description = null
) : IRequest<WorkflowDto>;