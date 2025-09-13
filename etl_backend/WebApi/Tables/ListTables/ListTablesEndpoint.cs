using Application.Tables.Queries;
using FastEndpoints;
using MediatR;

namespace WebApi.Tables.ListTables;

public class ListTablesEndpoint : EndpointWithoutRequest<List<TableListItem>>
{
    private readonly IMediator _mediator;

    public ListTablesEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("api/tables");
        Summary(s => s.Summary = "List all registered tables");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        Response = await _mediator.Send(new ListTablesQuery(), ct);
    }
}