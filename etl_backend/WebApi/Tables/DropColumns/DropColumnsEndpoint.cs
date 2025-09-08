using Application.Tables.Commands;
using FastEndpoints;
using MediatR;

namespace WebApi.Tables.DropColumns;

public class DropColumnsEndpoint : Endpoint<DropColumnsRequest>
{
    private readonly IMediator _mediator;

    public DropColumnsEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Delete("api/tables/{SchemaId:int}/columns");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Drop one or more columns";
            // s.Params(p => p.SchemaId, "Schema ID");
            s.ExampleRequest = new DropColumnsRequest
            {
                SchemaId = 1,
                ColumnIds = new List<int> { 1, 2, 3 }
            };
            s.ResponseExamples[200] = new { };
            s.ResponseExamples[400] = new { error = "ColumnIds are required." };
        });
    }

    public override async Task HandleAsync(DropColumnsRequest req, CancellationToken ct)
    {
        await _mediator.Send(new DropColumnsCommand(req.SchemaId, req.ColumnIds), ct);
        // await SendAsync(ct);
    }
}