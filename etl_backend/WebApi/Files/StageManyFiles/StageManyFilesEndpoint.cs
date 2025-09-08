using Application.Files.Commands;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using WebApi.Files;
using StageFileResponse = WebApi.Files.StageFileResponse;

/// <summary>
/// Stage multiple uploaded files for later processing
/// </summary>
public class StageManyFilesEndpoint : Endpoint<StageManyFilesRequest, List<StageManyFilesResponseItem>>
{
    private readonly IMediator _mediator;

    public StageManyFilesEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("api/files/stage-many");
        AllowAnonymous();
        AllowFormData();
        Summary(s =>
        {
            s.Summary = "Stage multiple files for ETL processing";
            s.Description = "Uploads and stages one or more files. Returns staging results for each file.";
            s.ExampleRequest = new StageManyFilesRequest
            {
                Files = new List<IFormFile>(), // Example — actual form data
                Subdir = "uploads"
            };
        });
    }

    public override async Task HandleAsync(StageManyFilesRequest req, CancellationToken ct)
    {
        var files = req.Files!
            .Where(f => f != null && f.Length > 0)
            .Select(f => new FileUploadItem(f.FileName, f.OpenReadStream()))
            .ToList();

        var command = new StageManyFilesCommand(files, req.Subdir ?? "uploads");

        var result = await _mediator.Send(command, ct);

        Response = result.Select(item => new StageManyFilesResponseItem
        {
            FileName = item.FileName,
            Success = item.Success,
            Error = item.Error,
            Data = item.Data == null ? null : new StageFileResponse
            {
                Id = item.Data.Id,
                OriginalFileName = item.Data.OriginalFileName,
                StoredFilePath = item.Data.StoredFilePath,
                FileSize = item.Data.FileSize,
                UploadedAt = item.Data.UploadedAt,
                Stage = item.Data.Stage,
                Status = item.Data.Status,
                ErrorCode = item.Data.ErrorCode,
                ErrorMessage = item.Data.ErrorMessage
            }
        }).ToList();
    }
}