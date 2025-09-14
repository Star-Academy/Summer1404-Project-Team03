using Application.Abstractions;
using Application.Files.Commands;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Files.Handlers;

public class StageManyFilesCommandHandler : IRequestHandler<StageManyFilesCommand, List<StageFileBatchItem>>
{
    private readonly IFileStagingService _staging;

    public StageManyFilesCommandHandler(IFileStagingService staging)
    {
        _staging = staging;
    }

    public async Task<List<StageFileBatchItem>> Handle(StageManyFilesCommand request, CancellationToken ct)
    {
        var results = new List<StageFileBatchItem>(request.Files.Count);

        foreach (var file in request.Files)
        {
            if (file.Content is null || file.Content.Length == 0)
            {
                results.Add(new StageFileBatchItem(
                    FileName: file.FileName,
                    Success: false,
                    Error: "Empty file."
                ));
                continue;
            }

            try
            {
                var staged = await _staging.StageAsync(file.Content, file.FileName, request.Subdirectory, ct);

                results.Add(new StageFileBatchItem(
                    FileName: file.FileName,
                    Success: true,
                    Data: Map(staged)
                ));
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                results.Add(new StageFileBatchItem(
                    FileName: file.FileName,
                    Success: false,
                    Error: ex.Message
                ));
            }
        }

        return results;
    }

    private static StageFileResponse Map(StagedFile s) => new(
        Id: s.Id,
        OriginalFileName: s.OriginalFileName,
        StoredFilePath: s.StoredFilePath,
        FileSize: s.FileSize,
        UploadedAt: s.UploadedAt,
        Stage: s.Stage.ToString(),
        Status: s.Status.ToString(),
        ErrorCode: s.ErrorCode == ProcessingErrorCode.None ? null : s.ErrorCode.ToString(),
        ErrorMessage: s.ErrorMessage
    );
}