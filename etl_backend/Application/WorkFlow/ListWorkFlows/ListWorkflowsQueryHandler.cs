using Application.Common.Exceptions;
using Application.Services.Abstractions;
using Application.WorkFlow.Abstractions;
using Application.WorkFlow.Dtos;
using MediatR;

namespace Application.WorkFlow.ListWorkFlows;

public class ListWorkflowsQueryHandler : IRequestHandler<ListWorkflowsQuery, List<WorkflowDto>>
{
    private readonly IWorkflowReader _reader;
    private readonly ICurrentUserService _currentUser;

    public ListWorkflowsQueryHandler(IWorkflowReader reader, ICurrentUserService currentUser)
    {
        _reader = reader;
        _currentUser = currentUser;
    }

    public async Task<List<WorkflowDto>> Handle(ListWorkflowsQuery request, CancellationToken ct)
    {
        if (!_currentUser.IsAuthenticated || string.IsNullOrWhiteSpace(_currentUser.UserId))
            throw new ForbiddenException("User not authenticated.");

        var workflows = await _reader.ListByUserAsync(_currentUser.UserId!, ct);
        return workflows.Select(w => new WorkflowDto(
            Id: w.Id,
            Name: w.Name,
            Description: w.Description,
            CreatedAt: w.CreatedAt,
            UpdatedAt: w.UpdatedAt,
            Status: w.Status.ToString()
        )).ToList();
    }
}