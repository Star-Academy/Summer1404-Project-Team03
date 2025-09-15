using Application.WorkFlow.DeleteWorkflow;
using FastEndpoints;
using MediatR;

namespace WebApi.Workflow.DeleteWorkflowEndpoint;

public class DeleteWorkflowEndpoint : EndpointWithoutRequest
{
    private readonly IMediator _mediator;

    public DeleteWorkflowEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Delete("api/workflows/{id}");
        Summary(s =>
        {
            s.Summary = "Delete workflow";
            s.Description = "Deletes the specified workflow.";
            // s.Params("Id", "Workflow ID", example: "workflow123");
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<string>("id");
        await _mediator.Send(new DeleteWorkflowCommand(id), ct);
        await SendNoContentAsync(ct);
    }
}