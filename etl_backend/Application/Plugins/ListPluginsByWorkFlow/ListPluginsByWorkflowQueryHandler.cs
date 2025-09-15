using Application.Common.Exceptions;
using Application.Plugins.Abstractions;
using Application.Plugins.Dtos;
using Application.Services.Abstractions;
using Application.WorkFlow.Abstractions;
using MediatR;

namespace Application.Plugins.ListPluginsByWorkFlow;

public class ListPluginsByWorkflowQueryHandler : IRequestHandler<ListPluginsByWorkflowQuery, List<PluginDto>>
{
    private readonly IWorkflowReader _workflowReader;
    private readonly IPluginReader _pluginReader;
    private readonly ICurrentUserService _currentUser;

    public ListPluginsByWorkflowQueryHandler(
        IWorkflowReader workflowReader,
        IPluginReader pluginReader,
        ICurrentUserService currentUser)
    {
        _workflowReader = workflowReader;
        _pluginReader = pluginReader;
        _currentUser = currentUser;
    }

    public async Task<List<PluginDto>> Handle(ListPluginsByWorkflowQuery request, CancellationToken ct)
    {
        if (!_currentUser.IsAuthenticated || string.IsNullOrWhiteSpace(_currentUser.UserId))
            throw new ForbiddenException("User not authenticated.");

        var workflow = await _workflowReader.GetByIdAsync(request.WorkflowId, _currentUser.UserId!, ct)
                       ?? throw new NotFoundException("Workflow", request.WorkflowId);

        var plugins = await _pluginReader.GetByWorkflowIdAsync(request.WorkflowId, _currentUser.UserId!, ct);
        return plugins.Select(p => new PluginDto(
            Id: p.Id,
            PluginType: p.PluginType,
            Config: p.Config,
            Order: p.Order,
            CreatedAt: p.CreatedAt,
            UpdatedAt: p.UpdatedAt
        )).ToList();
    }
}