using Application.Common.Exceptions;
using Application.Plugins.Abstractions;
using Application.Services.Abstractions;
using Application.WorkFlow.Abstractions;
using MediatR;

namespace Application.Plugins.ReorderPlugins;

public class ReorderPluginsCommandHandler : IRequestHandler<ReorderPluginsCommand>
{
    private readonly IWorkflowReader _workflowReader;
    private readonly IPluginReorderer _pluginReorderer;
    private readonly ICurrentUserService _currentUser;

    public ReorderPluginsCommandHandler(
        IWorkflowReader workflowReader,
        IPluginReorderer pluginReorderer,
        ICurrentUserService currentUser)
    {
        _workflowReader = workflowReader;
        _pluginReorderer = pluginReorderer;
        _currentUser = currentUser;
    }

    public async Task Handle(ReorderPluginsCommand request, CancellationToken ct)
    {
        if (!_currentUser.IsAuthenticated || string.IsNullOrWhiteSpace(_currentUser.UserId))
            throw new ForbiddenException("User not authenticated.");

        var workflow = await _workflowReader.GetByIdAsync(request.WorkflowId, _currentUser.UserId!, ct)
                       ?? throw new NotFoundException("Workflow", request.WorkflowId);

        await _pluginReorderer.ReorderAsync(request.WorkflowId, request.PluginIdsInOrder, _currentUser.UserId!, ct);
    }
}