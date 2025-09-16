using Application.Common.Authorization;
using Application.WorkFlow.Dtos;
using MediatR;

namespace Application.WorkFlow.UpdateWorkflow;

[RequireRole(AppRoles.Analyst, AppRoles.DataAdmin, AppRoles.SysAdmin)]
public record UpdateWorkflowCommand(
    string Id,
    string Name,
    string? Description = null,
    string? TableId = null, 
    string? Status = null
) : IRequest<WorkflowDto>;