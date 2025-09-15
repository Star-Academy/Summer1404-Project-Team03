using Application.Common.Authorization;
using MediatR;

namespace Application.WorkFlow.DeleteWorkflow;

[RequireRole(AppRoles.Analyst, AppRoles.DataAdmin, AppRoles.SysAdmin)]
public record DeleteWorkflowCommand(string Id) : IRequest;