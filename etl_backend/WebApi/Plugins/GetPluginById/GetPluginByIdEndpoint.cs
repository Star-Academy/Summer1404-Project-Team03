using Application.Plugins.Dtos;
using Application.Plugins.GetPluginById;
using FastEndpoints;
using MediatR;

namespace WebApi.Plugins.GetPluginById;

public class GetPluginEndpoint : EndpointWithoutRequest<PluginDto>
{
    private readonly IMediator _mediator;

    public GetPluginEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("api/workflows/{workflowId}/plugins/{pluginId}");
        Summary(s =>
        {
            s.Summary = "Get a plugin by ID";
            s.Description = "Returns the plugin configuration for the specified plugin ID.";
            // s.Params("WorkflowId", "Workflow ID", example: "workflow123");
            // s.Params("PluginId", "Plugin ID", example: "plugin456");
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var workflowId = Route<string>("workflowId");
        var pluginId = Route<string>("pluginId");
        var result = await _mediator.Send(new GetPluginByIdQuery(workflowId, pluginId), ct);
        Response = result;
    }
}