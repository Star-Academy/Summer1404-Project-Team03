using Application.Common.Exceptions;
using Application.Services.Abstractions;
using Application.WorkFlow.Abstractions;
using MediatR;

namespace Application.WorkFlow.DeleteWorkflow;

public class DeleteWorkflowCommandHandler : IRequestHandler<DeleteWorkflowCommand>
{
    private readonly IWorkflowDeleter _deleter;
    private readonly ICurrentUserService _currentUser;

    public DeleteWorkflowCommandHandler(IWorkflowDeleter deleter, ICurrentUserService currentUser)
    {
        _deleter = deleter;
        _currentUser = currentUser;
    }

    public async Task Handle(DeleteWorkflowCommand request, CancellationToken ct)
    {
        if (!_currentUser.IsAuthenticated || string.IsNullOrWhiteSpace(_currentUser.UserId))
            throw new ForbiddenException("User not authenticated.");

        await _deleter.DeleteAsync(request.Id, _currentUser.UserId!, ct);
    }
}