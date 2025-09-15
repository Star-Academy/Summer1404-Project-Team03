using Application.Common.Exceptions;
using Application.Plugins.Abstractions;
using Application.Plugins.Dtos;
using Application.Services.Abstractions;
using Application.WorkFlow.Abstractions;
using MediatR;

namespace Application.Plugins.UpdatePlugin;

public class UpdatePluginCommandHandler : IRequestHandler<UpdatePluginCommand, PluginDto>
{
    private readonly IWorkflowReader _workflowReader;
    private readonly IPluginReader _pluginReader;
    private readonly IPluginWriter _pluginWriter;
    private readonly ICurrentUserService _currentUser;
    private readonly IPluginConfigParser _configParser;

    public UpdatePluginCommandHandler(
        IWorkflowReader workflowReader,
        IPluginReader pluginReader,
        IPluginWriter pluginWriter,
        ICurrentUserService currentUser,
        IPluginConfigParser configParser)
    {
        _workflowReader = workflowReader;
        _pluginReader = pluginReader;
        _pluginWriter = pluginWriter;
        _currentUser = currentUser;
        _configParser = configParser;
    }

    public async Task<PluginDto> Handle(UpdatePluginCommand request, CancellationToken ct)
    {
        if (!_currentUser.IsAuthenticated || string.IsNullOrWhiteSpace(_currentUser.UserId))
            throw new ForbiddenException("User not authenticated.");

        var plugin = await _pluginReader.GetByIdAsync(request.PluginId, _currentUser.UserId!, ct)
                     ?? throw new NotFoundException("Plugin", request.PluginId);

        var config = _configParser.Parse(request.PluginType, request.Config);
        plugin.Update(request.PluginType, config);
        await _pluginWriter.UpdateAsync(plugin, ct);

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