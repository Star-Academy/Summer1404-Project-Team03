using Application.WorkFlow.GetWorkflowById;
using FastEndpoints;
using MediatR;

namespace WebApi.Workflow.GetWorkflow;

public class GetWorkflowEndpoint : EndpointWithoutRequest<GetWorkflowResponse>
{
    private readonly IMediator _mediator;

    public GetWorkflowEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("api/workflows/{id}");
        Summary(s =>
        {
            s.Summary = "Get workflow by ID";
            s.Description = "Returns workflow details for the specified ID.";
            // s.Params("Id", "Workflow ID", example: "workflow123");
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<string>("id");
        var result = await _mediator.Send(new GetWorkflowByIdQuery(id), ct);

        Response = new GetWorkflowResponse
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