using Application.Plugins.Dtos;
using Application.Plugins.ListPluginsByWorkFlow;
using FastEndpoints;
using MediatR;

namespace WebApi.Plugins.ListPlugins;

public class ListPluginsEndpoint : EndpointWithoutRequest<List<PluginDto>>
{
    private readonly IMediator _mediator;

    public ListPluginsEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("api/workflows/{workflowId}/plugins");
        Summary(s =>
        {
            s.Summary = "List all plugins in a workflow";
            s.Description = "Returns all plugins for the specified workflow, ordered by position.";
            // s.Params("WorkflowId", "Workflow ID", example: "workflow123");
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var workflowId = Route<string>("workflowId");
        var result = await _mediator.Send(new ListPluginsByWorkflowQuery(workflowId), ct);
        Response = result;
    }
}