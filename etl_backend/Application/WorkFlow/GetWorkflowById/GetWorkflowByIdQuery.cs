using Application.Common.Authorization;
using Application.WorkFlow.Dtos;
using MediatR;

namespace Application.WorkFlow.GetWorkflowById;

[RequireRole(AppRoles.Analyst, AppRoles.DataAdmin, AppRoles.SysAdmin)]
public record GetWorkflowByIdQuery(string Id) : IRequest<WorkflowDto>;