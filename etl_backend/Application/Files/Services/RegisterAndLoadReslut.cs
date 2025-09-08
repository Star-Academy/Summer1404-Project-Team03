using Application.Files.Commands;

namespace Application.Files.Services;

public record RegisterAndLoadResult(
    int SchemaId,
    string TableName,
    List<SchemaColumnDto> Columns,
    StagedFileStatusDto Staged,
    LoadResult Load
);