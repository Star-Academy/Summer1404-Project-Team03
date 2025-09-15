using Application.WorkFlow.UpdateWorkflow;
using FastEndpoints;
using MediatR;

namespace WebApi.Workflow.UpdateWorkflow;

public class UpdateWorkflowEndpoint : Endpoint<UpdateWorkflowRequest, UpdateWorkflowResponse>
{
    private readonly IMediator _mediator;

    public UpdateWorkflowEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Put("api/workflows/{id}");
        Summary(s =>
        {
            s.Summary = "Update workflow";
            s.Description = "Updates workflow name, description, or status.";
            // s.Params("Id", "Workflow ID", example: "workflow123");
            s.ExampleRequest = new UpdateWorkflowRequest
            {
                Name = "Updated Name",
                Description = "Updated description",
                Status = "Running"
            };
        });
    }

    public override async Task HandleAsync(UpdateWorkflowRequest req, CancellationToken ct)
    {
        var id = Route<string>("id");
        var result = await _mediator.Send(new UpdateWorkflowCommand(
            id,
            req.Name,
            req.Description,
            req.Status
        ), ct);

        Response = new UpdateWorkflowResponse
        {
            Id = result.Id,
            Name = result.Name,
            Description = result.Description,
            CreatedAt = result.CreatedAt,
            UpdatedAt = result.UpdatedAt,
            Status = result.Status
        };
    }
}