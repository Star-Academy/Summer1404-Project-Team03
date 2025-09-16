using Application.WorkFlow.ListWorkFlows;
using FastEndpoints;
using MediatR;

namespace WebApi.Workflow.ListWorkFlows;

public class ListWorkflowsEndpoint : EndpointWithoutRequest<ListWorkflowsResponse>
{
    private readonly IMediator _mediator;

    public ListWorkflowsEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("api/workflows");
        Summary(s =>
        {
            s.Summary = "List all workflows for current user";
            s.Description = "Returns list of workflows owned by the authenticated user.";
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var result = await _mediator.Send(new ListWorkflowsQuery(), ct);

        Response = new ListWorkflowsResponse
        {
            Workflows = result.Select(w => new WorkflowItem
            {
                Id = w.Id,
                Name = w.Name,
                Description = w.Description,
                TableId = w.TableId,
                CreatedAt = w.CreatedAt,
                UpdatedAt = w.UpdatedAt,
                Status = w.Status
            }).ToList()
        };
    }
}