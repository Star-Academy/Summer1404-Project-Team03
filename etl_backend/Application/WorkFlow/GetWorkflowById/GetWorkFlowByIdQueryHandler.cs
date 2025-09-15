using Application.Common.Exceptions;
using Application.Services.Abstractions;
using Application.WorkFlow.Abstractions;
using Application.WorkFlow.Dtos;
using MediatR;

namespace Application.WorkFlow.GetWorkflowById;

public class GetWorkflowByIdQueryHandler : IRequestHandler<GetWorkflowByIdQuery, WorkflowDto>
{
    private readonly IWorkflowReader _reader;
    private readonly ICurrentUserService _currentUser;

    public GetWorkflowByIdQueryHandler(IWorkflowReader reader, ICurrentUserService currentUser)
    {
        _reader = reader;
        _currentUser = currentUser;
    }

    public async Task<WorkflowDto> Handle(GetWorkflowByIdQuery request, CancellationToken ct)
    {
        if (!_currentUser.IsAuthenticated || string.IsNullOrWhiteSpace(_currentUser.UserId))
            throw new ForbiddenException("User not authenticated.");

        var workflow = await _reader.GetByIdAsync(request.Id, _currentUser.UserId!, ct)
                       ?? throw new NotFoundException("Workflow", request.Id);

        return new WorkflowDto(
            Id: workflow.Id,
            Name: workflow.Name,
            Description: workflow.Description,
            CreatedAt: workflow.CreatedAt,
            UpdatedAt: workflow.UpdatedAt,
            Status: workflow.Status.ToString()
        );
    }
}