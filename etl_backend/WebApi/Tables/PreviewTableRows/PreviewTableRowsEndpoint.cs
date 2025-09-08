using Application.Tables.Queries;
using FastEndpoints;
using MediatR;

namespace WebApi.Tables.PreviewTableRows;

public class PreviewTableRowsEndpoint : Endpoint<PreviewTableRowsRequest, PreviewTableRowsResponse>
{
    private readonly IMediator _mediator;

    public PreviewTableRowsEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("api/tables/{SchemaId:int}/rows");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Preview rows from a table";
            s.Description = "Returns a paginated, optionally sorted subset of table rows.";
            // s.Params(p => p.SchemaId, "The schema ID of the table");
            s.ExampleRequest = new PreviewTableRowsRequest
            {
                SchemaId = 1,
                Offset = 0,
                Limit = 10,
                OrderBy = "id",
                Direction = "asc"
            };
        });
    }

    public override async Task HandleAsync(PreviewTableRowsRequest req, CancellationToken ct)
    {
        var result = await _mediator.Send(new PreviewTableRowsQuery(
            req.SchemaId,
            req.Offset,
            req.Limit,
            req.OrderBy,
            req.Direction
        ), ct);

        Response = new PreviewTableRowsResponse
        {
            Rows = result.Rows,
            NextOffset = result.NextOffset
        };
    }
}