using Application.Plugins.DeletePlugin;
using FastEndpoints;
using MediatR;

namespace WebApi.Plugins.DeletePlugin;

public class DeletePluginEndpoint : EndpointWithoutRequest
{
    private readonly IMediator _mediator;

    public DeletePluginEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Delete("api/workflows/{workflowId}/plugins/{pluginId}");
        Summary(s =>
        {
            s.Summary = "Delete a plugin";
            s.Description = "Deletes the specified plugin from the workflow.";
            // s.Params("WorkflowId", "Workflow ID", example: "workflow123");
            // s.Params("PluginId", "Plugin ID", example: "plugin456");
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var workflowId = Route<string>("workflowId");
        var pluginId = Route<string>("pluginId");
        await _mediator.Send(new DeletePluginCommand(workflowId, pluginId), ct);
        await SendNoContentAsync(ct);
    }
}