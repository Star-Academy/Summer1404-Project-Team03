using Application.Files.Queries;
using FastEndpoints;
using MediatR;

namespace WebApi.Files;

/// <summary>
/// List all staged files
/// </summary>
/// <remarks>
/// Returns summary of all staged files including status, size, and upload time.
/// </remarks>
public class ListStagedFilesEndpoint : EndpointWithoutRequest<ListStagedFilesResponse>
{
    private readonly IMediator _mediator;

    public ListStagedFilesEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("api/files");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "List all staged files";
            s.Description = "Returns summary of all staged files including status, size, and upload time.";
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var result = await _mediator.Send(new ListStagedFilesQuery(), ct);

        Response = new ListStagedFilesResponse
        {
            Items = result.Select(item => new ListStagedFilesItemResponse
            {
                Id = item.Id,
                OriginalFileName = item.OriginalFileName,
                Stage = item.Stage,
                Status = item.Status,
                SchemaId = item.SchemaId,
                FileSize = item.FileSize,
                UploadedAt = item.UploadedAt
            }).ToList()
        };
    }
}