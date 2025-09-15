using Application.Common.Exceptions;
using Application.Plugins.Abstractions;
using Application.Plugins.Dtos;
using Application.Services.Abstractions;
using Application.WorkFlow.Abstractions;
using MediatR;

namespace Application.Plugins.GetPluginById;

public class GetPluginByIdQueryHandler : IRequestHandler<GetPluginByIdQuery, PluginDto>
{
    private readonly IWorkflowReader _workflowReader;
    private readonly IPluginReader _pluginReader;
    private readonly ICurrentUserService _currentUser;

    public GetPluginByIdQueryHandler(
        IWorkflowReader workflowReader,
        IPluginReader pluginReader,
        ICurrentUserService currentUser)
    {
        _workflowReader = workflowReader;
        _pluginReader = pluginReader;
        _currentUser = currentUser;
    }

    public async Task<PluginDto> Handle(GetPluginByIdQuery request, CancellationToken ct)
    {
        if (!_currentUser.IsAuthenticated || string.IsNullOrWhiteSpace(_currentUser.UserId))
            throw new ForbiddenException("User not authenticated.");

        var workflow = await _workflowReader.GetByIdAsync(request.WorkflowId, _currentUser.UserId!, ct)
                       ?? throw new NotFoundException("Workflow", request.WorkflowId);

        var plugin = await _pluginReader.GetByIdAsync(request.PluginId, _currentUser.UserId!, ct)
                     ?? throw new NotFoundException("Plugin", request.PluginId);

        return new PluginDto(
            Id: plugin.Id,
            PluginType: plugin.PluginType,
            Config: plugin.Config,
            Order: plugin.Order,
            CreatedAt: plugin.CreatedAt,
            UpdatedAt: plugin.UpdatedAt
        );
    }
}