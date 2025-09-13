using Application.Tables.Queries;
using FastEndpoints;
using MediatR;

namespace WebApi.Tables.ListColumns;

public class ListColumnsEndpoint : Endpoint<ListColumnsRequest, ListColumnsResponse>
{
    private readonly IMediator _mediator;

    public ListColumnsEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("api/tables/{SchemaId:int}/columns");
        Summary(s =>
        {
            s.Summary = "List columns for a table";
            // s.Params(p => p.SchemaId, "Schema ID");
            s.ExampleRequest = new ListColumnsRequest { SchemaId = 1 };
        });
    }

    public override async Task HandleAsync(ListColumnsRequest req, CancellationToken ct)
    {
        var result = await _mediator.Send(new ListColumnsQuery(req.SchemaId), ct);
        Response = new ListColumnsResponse
        {
            Items = result.Select(c => new ColumnListItem
            {
                Id = c.Id,
                OrdinalPosition = c.OrdinalPosition,
                Name = c.Name,
                Type = c.Type,
                OriginalName = c.OriginalName
            }).ToList()
        };
    }
}