using Application.Common.Exceptions;
using Application.Services.Abstractions;
using Application.WorkFlow.Abstractions;
using Application.WorkFlow.Dtos;
using Domain.Enums;
using MediatR;

namespace Application.WorkFlow.UpdateWorkflow;

public class UpdateWorkflowCommandHandler : IRequestHandler<UpdateWorkflowCommand, WorkflowDto>
{
    private readonly IWorkflowReader _reader;
    private readonly IWorkflowWriter _writer;
    private readonly ICurrentUserService _currentUser;

    public UpdateWorkflowCommandHandler(IWorkflowReader reader, IWorkflowWriter writer, ICurrentUserService currentUser)
    {
        _reader = reader;
        _writer = writer;
        _currentUser = currentUser;
    }

    public async Task<WorkflowDto> Handle(UpdateWorkflowCommand request, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new UnprocessableEntityException("Name is required.");

        if (!_currentUser.IsAuthenticated || string.IsNullOrWhiteSpace(_currentUser.UserId))
            throw new ForbiddenException("User not authenticated.");

        var workflow = await _reader.GetByIdAsync(request.Id, _currentUser.UserId!, ct)
                       ?? throw new NotFoundException("Workflow", request.Id);

        var status = Enum.TryParse<WorkflowStatus>(request.Status, true, out var parsedStatus)
            ? parsedStatus
            : workflow.Status;

        workflow.Update(request.Name, request.Description, status,request.TableId); 
        await _writer.UpdateAsync(workflow, ct);

        return new WorkflowDto(
            Id: workflow.Id,
            Name: workflow.Name,
            Description: workflow.Description,
            TableId: workflow.TableId,
            CreatedAt: workflow.CreatedAt,
            UpdatedAt: workflow.UpdatedAt,
            Status: workflow.Status.ToString()
        );
    }
}