using Application.Plugins.ReorderPlugins;
using FastEndpoints;
using MediatR;

namespace WebApi.Plugins.ReorderPlugins;

public class ReorderPluginsEndpoint : Endpoint<ReorderPluginsRequest>
{
    private readonly IMediator _mediator;

    public ReorderPluginsEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Patch("api/workflows/{workflowId}/plugins/reorder");
        Summary(s =>
        {
            s.Summary = "Reorder plugins in a workflow";
            s.Description = "Reorders the plugins in the specified workflow.";
            // s.Params("WorkflowId", "Workflow ID", example: "workflow123");
            s.ExampleRequest = new ReorderPluginsRequest
            {
                PluginIdsInOrder = new List<string> { "plugin1", "plugin2", "plugin3" }
            };
        });
    }

    public override async Task HandleAsync(ReorderPluginsRequest req, CancellationToken ct)
    {
        var workflowId = Route<string>("workflowId");
        await _mediator.Send(new ReorderPluginsCommand(workflowId, req.PluginIdsInOrder), ct);
        await SendNoContentAsync(ct);
    }
}