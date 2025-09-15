using Application.Common.Exceptions;
using Application.Services.Abstractions;
using Application.WorkFlow.Abstractions;
using Application.WorkFlow.Dtos;
using Domain.Entities;
using MediatR;

namespace Application.WorkFlow.CreateWorkflow;

public class CreateWorkflowCommandHandler : IRequestHandler<CreateWorkflowCommand, WorkflowDto>
{
    private readonly IWorkflowWriter _writer;
    private readonly ICurrentUserService _currentUser;

    public CreateWorkflowCommandHandler(IWorkflowWriter writer, ICurrentUserService currentUser)
    {
        _writer = writer;
        _currentUser = currentUser;
    }

    public async Task<WorkflowDto> Handle(CreateWorkflowCommand request, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new UnprocessableEntityException("Name is required.");

        if (!_currentUser.IsAuthenticated || string.IsNullOrWhiteSpace(_currentUser.UserId))
            throw new ForbiddenException("User not authenticated.");

        var workflow = new Workflow(
            id: Guid.NewGuid().ToString(),
            userId: _currentUser.UserId!,
            name: request.Name,
            description: request.Description
        );

        await _writer.AddAsync(workflow, ct);

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