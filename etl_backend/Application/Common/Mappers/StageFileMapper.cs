using Application.Files.Commands;
using Domain.Entities;
using Domain.Enums;

namespace Application.Common.Mappers;

internal static class StageFileMapper
{
    public static StageFileResponse Map(StagedFile s) => new(
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