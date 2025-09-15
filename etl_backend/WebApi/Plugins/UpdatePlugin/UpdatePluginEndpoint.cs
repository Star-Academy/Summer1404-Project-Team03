using Application.Plugins.Dtos;
using Application.Plugins.UpdatePlugin;
using Domain.Enums;
using FastEndpoints;
using MediatR;

namespace WebApi.Plugins.UpdatePlugin;

public class UpdatePluginEndpoint : Endpoint<UpdatePluginRequest, PluginDto>
{
    private readonly IMediator _mediator;

    public UpdatePluginEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Put("api/workflows/{workflowId}/plugins/{pluginId}");
        Summary(s =>
        {
            s.Summary = "Update a plugin";
            s.Description = "Updates plugin configuration.";
            // s.Params("WorkflowId", "Workflow ID");
            // s.Params("PluginId", "Plugin ID");
            s.ExampleRequest = new UpdatePluginRequest
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
                            ["Value"] = "21"
                        }
                    }
                }
            };
        });
    }

    public override async Task HandleAsync(UpdatePluginRequest req, CancellationToken ct)
    {
        var workflowId = Route<string>("workflowId");
        var pluginId = Route<string>("pluginId");

        var result = await _mediator.Send(new UpdatePluginCommand(
            workflowId,
            pluginId,
            req.PluginType,
            req.Config
        ), ct);

        Response = result;
    }
}