using Application.Tables.Commands;
using FastEndpoints;
using MediatR;

namespace WebApi.Tables.RenameColumn;

public class RenameColumnEndpoint : Endpoint<RenameColumnRequest>
{
    private readonly IMediator _mediator;

    public RenameColumnEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("api/tables/{SchemaId:int}/columns/{ColumnId:int}/rename");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Rename a column";
            // s.Params(p => p.SchemaId, "Schema ID");
            // s.Params(p => p.ColumnId, "Column ID");
            s.ExampleRequest = new RenameColumnRequest { SchemaId = 1, ColumnId = 2, NewName = "new_name" };
        });
    }

    public override async Task HandleAsync(RenameColumnRequest req, CancellationToken ct)
    {
        await _mediator.Send(new RenameColumnCommand(req.SchemaId, req.ColumnId, req.NewName), ct);
        // await SendAsync(ct);
    }
}