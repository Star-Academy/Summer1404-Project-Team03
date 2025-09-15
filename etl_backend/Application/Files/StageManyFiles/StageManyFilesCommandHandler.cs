using Application.Common.Mappers;
using Application.Files.Commands;
using Application.Files.StageManyFiles.ServiceAbstractions;
using MediatR;

namespace Application.Files.StageManyFiles;
public class StageManyFilesCommandHandler : IRequestHandler<StageManyFilesCommand, List<StageFileBatchItem>>
{
    private readonly IAddStageFileService _staging;
    public StageManyFilesCommandHandler(IAddStageFileService staging)
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
                    Data: StageFileMapper.Map(staged)
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
}