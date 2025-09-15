using Application.Common.Exceptions;
using Application.Plugins.Abstractions;
using Application.Services.Abstractions;
using Application.WorkFlow.Abstractions;
using MediatR;

namespace Application.Plugins.DeletePlugin;

public class DeletePluginCommandHandler : IRequestHandler<DeletePluginCommand>
{
    private readonly IWorkflowReader _workflowReader;
    private readonly IPluginDeleter _pluginDeleter;
    private readonly ICurrentUserService _currentUser;

    public DeletePluginCommandHandler(
        IWorkflowReader workflowReader,
        IPluginDeleter pluginDeleter,
        ICurrentUserService currentUser)
    {
        _workflowReader = workflowReader;
        _pluginDeleter = pluginDeleter;
        _currentUser = currentUser;
    }

    public async Task Handle(DeletePluginCommand request, CancellationToken ct)
    {
        if (!_currentUser.IsAuthenticated || string.IsNullOrWhiteSpace(_currentUser.UserId))
            throw new ForbiddenException("User not authenticated.");

        var workflow = await _workflowReader.GetByIdAsync(request.WorkflowId, _currentUser.UserId!, ct)
                       ?? throw new NotFoundException("Workflow", request.WorkflowId);

        await _pluginDeleter.DeleteAsync(request.PluginId, _currentUser.UserId!, ct);
    }
}