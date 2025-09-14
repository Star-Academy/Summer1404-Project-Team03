using Application.Files.Commands;

namespace Application.Dtos;

public record RegisterAndLoadResult(
    int SchemaId,
    string TableName,
    List<SchemaColumnDto> Columns,
    StagedFileStatusDto Staged,
    LoadResult Load
);