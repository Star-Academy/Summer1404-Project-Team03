using Application.Plugins.AddPlugin;
using Application.Plugins.Dtos;
using Domain.Enums;
using FastEndpoints;
using MediatR;

namespace WebApi.Plugins.AddPlugin;

public class AddPluginEndpoint : Endpoint<AddPluginRequest, PluginDto>
{
    private readonly IMediator _mediator;

    public AddPluginEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("api/workflows/{workflowId}/plugins");
        Summary(s =>
        {
            s.Summary = "Add a plugin";
            s.Description = "Adds a new plugin to workflow.";
            // s.Params("WorkflowId", "Workflow ID");
            s.ExampleRequest = new AddPluginRequest
            {
                PluginType = PluginType.Filter,
                Config = new Dictionary<string, object>
                {
                    ["Conditions"] = new[]
                    {
                        new Dictionary<string, object>
                        {
                            ["Column"] = "age",
                            ["Op"] = "Gt",
                            ["TypeHint"] = "Int",
                            ["Value"] = "18"
                        }
                    }
                }
            };
        });
    }

    public override async Task HandleAsync(AddPluginRequest req, CancellationToken ct)
    {
        var workflowId = Route<string>("workflowId");

        var result = await _mediator.Send(new AddPluginCommand(
            workflowId,
            req.PluginType,
            req.Config,
            req.Order
        ), ct);

        Response = result;
    }
}