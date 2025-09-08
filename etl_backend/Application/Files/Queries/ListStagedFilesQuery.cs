using MediatR;

namespace Application.Files.Queries;

public record ListStagedFilesQuery : IRequest<List<ListFilesItem>>;

public record ListFilesItem(
    int Id,
    string OriginalFileName,
    string Stage,
    string Status,
    int? SchemaId,
    long FileSize,          
    DateTime UploadedAt      
);