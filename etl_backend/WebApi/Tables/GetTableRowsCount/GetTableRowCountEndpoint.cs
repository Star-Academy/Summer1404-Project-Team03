using Application.Tables.Queries;
using FastEndpoints;
using MediatR;

namespace WebApi.Tables.GetTableRowsCount;

public class GetTableRowCountEndpoint : Endpoint<GetTableRowCountRequest, GetTableRowCountResponse>
{
    private readonly IMediator _mediator;

    public GetTableRowCountEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("api/tables/{SchemaId:int}/count");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Get row count for a table";
            s.Description = "Returns approximate or exact row count.";
            // s.Params(p => p.SchemaId, "The schema ID of the table");
            s.ExampleRequest = new GetTableRowCountRequest { SchemaId = 1, Exact = true };
        });
    }

    public override async Task HandleAsync(GetTableRowCountRequest req, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetTableRowCountQuery(req.SchemaId, req.Exact), ct);
        Response = new GetTableRowCountResponse
        {
            Exact = result.Exact,
            Count = result.Count
        };
    }
}