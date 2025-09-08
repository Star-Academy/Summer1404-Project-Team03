using Application.Files.Commands;
using FastEndpoints;
using MediatR;

namespace WebApi.Files;

/// <summary>
/// Load staged file data into its registered table
/// </summary>
/// <remarks>
/// Starts ETL load process. Supports Append or Truncate modes.
/// </remarks>
public class LoadFileIntoTableEndpoint : Endpoint<LoadFileIntoTableRequest, LoadFileIntoTableResponse>
{
    private readonly IMediator _mediator;

    public LoadFileIntoTableEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("api/files/{Id}/load");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Load staged file into database table";
            s.Description = "Starts data load process. Supports Append or Truncate modes.";
            s.ExampleRequest = new LoadFileIntoTableRequest
            {
                Id = 1,
                Mode = "Append",
                DropOnFailure = false
            };
        });
    }

    public override async Task HandleAsync(LoadFileIntoTableRequest req, CancellationToken ct)
    {
        var command = new LoadFileIntoTableCommand(
            req.Id,
            (LoadMode)Enum.Parse(typeof(LoadMode), req.Mode, true),
            req.DropOnFailure
        );

        var result = await _mediator.Send(command, ct);

        Response = new LoadFileIntoTableResponse
        {
            RowsInserted = result.RowsInserted,
            ElapsedMs = result.ElapsedMs
        };
    }
}