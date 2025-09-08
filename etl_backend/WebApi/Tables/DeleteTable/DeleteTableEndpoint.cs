using Application.Tables.Commands;
using FastEndpoints;
using MediatR;

namespace WebApi.Tables.DeleteTable;

public class DeleteTableEndpoint : EndpointWithoutRequest
{
    private readonly IMediator _mediator;

    public DeleteTableEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Delete("api/tables/{schemaId}");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Delete a table";
            // s.Params("schemaId", "Schema ID of the table to delete", example: 1);
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<int>("schemaId");
        await _mediator.Send(new DeleteTableCommand(id), ct);
        // await SendAsync(ct);
    }
}