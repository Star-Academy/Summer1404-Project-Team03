using Application.Common.Exceptions;
using Application.Plugins.Abstractions;
using Application.Plugins.Dtos;
using Application.Services.Abstractions;
using Application.WorkFlow.Abstractions;
using Domain.Entities;
using MediatR;

namespace Application.Plugins.AddPlugin;

public class AddPluginCommandHandler : IRequestHandler<AddPluginCommand, PluginDto>
{
    private readonly IWorkflowReader _workflowReader;
    private readonly IPluginWriter _pluginWriter;
    private readonly ICurrentUserService _currentUser;
    private readonly IPluginConfigParser _configParser;

    public AddPluginCommandHandler(
        IWorkflowReader workflowReader,
        IPluginWriter pluginWriter,
        ICurrentUserService currentUser,
        IPluginConfigParser configParser)
    {
        _workflowReader = workflowReader;
        _pluginWriter = pluginWriter;
        _currentUser = currentUser;
        _configParser = configParser;
    }

    public async Task<PluginDto> Handle(AddPluginCommand request, CancellationToken ct)
    {
        if (!_currentUser.IsAuthenticated || string.IsNullOrWhiteSpace(_currentUser.UserId))
            throw new ForbiddenException("User not authenticated.");

        var workflow = await _workflowReader.GetByIdAsync(request.WorkflowId, _currentUser.UserId!, ct)
                       ?? throw new NotFoundException("Workflow", request.WorkflowId);

        var config = _configParser.Parse(request.PluginType, request.Config);

        var plugin = new Plugin(
            id: Guid.NewGuid().ToString(),
            pluginType: request.PluginType,
            config: config,
            order: 1
        );

        workflow.AddPlugin(plugin, request.Order);
        await _pluginWriter.AddAsync(plugin, ct);

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