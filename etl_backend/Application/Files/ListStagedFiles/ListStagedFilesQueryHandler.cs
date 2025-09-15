using Application.Files.Queries;
using Application.Services.Repositories.Abstractions;
using MediatR;

namespace Application.Files.Handlers;

public class ListStagedFilesQueryHandler : IRequestHandler<ListStagedFilesQuery, List<ListFilesItem>>
{
    private readonly IStagedFileRepository _stagedRepo;

    public ListStagedFilesQueryHandler(IStagedFileRepository stagedRepo)
    {
        _stagedRepo = stagedRepo;
    }

    public async Task<List<ListFilesItem>> Handle(ListStagedFilesQuery request, CancellationToken ct)
    {
        var items = await _stagedRepo.ListAsync(ct);

        return items.Select(s => new ListFilesItem(
            Id: s.Id,
            OriginalFileName: s.OriginalFileName,
            Stage: s.Stage.ToString(),
            Status: s.Status.ToString(),
            SchemaId: s.SchemaId,
            FileSize: s.FileSize,          
            UploadedAt: s.UploadedAt       
        )).ToList();
    }
}