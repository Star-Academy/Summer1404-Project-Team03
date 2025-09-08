using MediatR;

namespace Application.Files.Commands;

public record StageManyFilesCommand(
    List<FileUploadItem> Files,  
    string Subdirectory = "uploads"
) : IRequest<List<StageFileBatchItem>>;

public record FileUploadItem(
    string FileName,
    Stream Content
);

public record StageFileBatchItem(
    string FileName,
    bool Success,
    string? Error = null,
    StageFileResponse? Data = null
);

public record StageFileResponse(
    int Id,
    string OriginalFileName,
    string StoredFilePath,
    long FileSize,
    DateTime UploadedAt,
    string Stage,
    string Status,
    string? ErrorCode,
    string? ErrorMessage
);