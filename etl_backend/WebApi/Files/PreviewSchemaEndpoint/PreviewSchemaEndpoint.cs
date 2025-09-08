using Application.Files.Queries;
using FastEndpoints;
using MediatR;

namespace WebApi.Files;


/// <summary>
/// Preview column schema from a staged file
/// </summary>
/// <remarks>
/// Reads the header row of a staged file and returns proposed column schema.
/// </remarks>

public class PreviewSchemaEndpoint : Endpoint<PreviewSchemaRequest, PreviewSchemaResponse>
{
    private readonly IMediator _mediator;

    public PreviewSchemaEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("api/files/{Id}/schema/preview");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Preview schema from staged file";
            s.Description = "Returns column schema preview based on header row of the staged file.";
            s.ExampleRequest = new PreviewSchemaRequest { Id = 1 };
        });
    }

    public override async Task HandleAsync(PreviewSchemaRequest req, CancellationToken ct)
    {
        var result = await _mediator.Send(new PreviewSchemaQuery(req.Id), ct);

        Response = new PreviewSchemaResponse
        {
            StagedFileId = result.StagedFileId,
            Columns = result.Columns.Select(c => new PreviewSchemaColumnItem
            {
                OrdinalPosition = c.OrdinalPosition,
                ColumnName = c.ColumnName,
                OriginalColumnName = c.OriginalColumnName
            }).ToList()
        };
    }
}