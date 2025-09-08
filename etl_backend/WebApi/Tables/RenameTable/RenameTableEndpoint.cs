using Application.Tables.Commands;
using FastEndpoints;
using MediatR;

namespace WebApi.Tables.RenameTable;

public class RenameTableEndpoint : Endpoint<RenameTableRequest>
{
    private readonly IMediator _mediator;

    public RenameTableEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("api/tables/{SchemaId}/rename");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Rename a table";
            // s.Params(p => p.SchemaId, "Schema ID of the table to rename");
            s.ExampleRequest = new RenameTableRequest { NewTableName = "new_name" };
        });
    }

    public override async Task HandleAsync(RenameTableRequest req, CancellationToken ct)
    {
        await _mediator.Send(new RenameTableCommand(Route<int>("SchemaId"), req.NewTableName), ct);
        // await SendAsync(ct);
    }
}