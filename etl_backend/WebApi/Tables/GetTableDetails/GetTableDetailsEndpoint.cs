using Application.Tables.Queries;
using FastEndpoints;
using MediatR;

namespace WebApi.Tables.GetTableDetails;

public class GetTableDetailsEndpoint : Endpoint<GetTableDetailsRequest, GetTableDetailsResponse>
{
    private readonly IMediator _mediator;

    public GetTableDetailsEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("api/tables/{SchemaId:int}/details");
        Summary(s =>
        {
            s.Summary = "Get detailed information about a table";
            s.Description = "Returns schema metadata, physical existence, row count estimate, and size.";
            // s.Params(p => p.SchemaId, "The schema ID of the table");
            s.ExampleRequest = new GetTableDetailsRequest { SchemaId = 1 };
        });
    }

    public override async Task HandleAsync(GetTableDetailsRequest req, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetTableDetailsQuery(req.SchemaId), ct);
        Response = new GetTableDetailsResponse
        {
            SchemaId = result.SchemaId,
            TableName = result.TableName,
            PhysicalExists = result.PhysicalExists,
            RowCountApprox = result.RowCountApprox,
            SizeBytes = result.SizeBytes,
            Columns = result.Columns.Select(c => new ColumnDetailsDto
            {
                Ordinal = c.Ordinal,
                Name = c.Name,
                Type = c.Type
            }).ToList()
        };
    }
}