using Application.WorkFlow.CreateWorkflow;
using Azure;
using FastEndpoints;
using MediatR;

namespace WebApi.Workflow;

public class CreateWorkflowEndpoint : Endpoint<CreateWorkflowRequest, CreateWorkflowResponse>
{
    private readonly IMediator _mediator;

    public CreateWorkflowEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("api/workflows");
        Summary(s =>
        {
            s.Summary = "Create a new workflow";
            s.Description = "Creates a new workflow for the authenticated user. TableId is optional.";
            s.ExampleRequest = new CreateWorkflowRequest
            {
                Name = "Customer Analysis",
                Description = "Analyze customer data",
                TableId = "table123" 
            };
        });
    }

    public override async Task HandleAsync(CreateWorkflowRequest req, CancellationToken ct)
    {
        var result = await _mediator.Send(new CreateWorkflowCommand(
            req.Name,
            req.Description
        ), ct);

        Response = new CreateWorkflowResponse
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