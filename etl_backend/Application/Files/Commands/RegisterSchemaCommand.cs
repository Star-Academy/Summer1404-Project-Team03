using MediatR;

namespace Application.Files.Commands;

public record RegisterSchemaCommand(
    int StagedFileId,
    Dictionary<int, String> ColumnTypeMap
) : IRequest<RegisterSchemaResult>;

public record RegisterSchemaResult(
    int SchemaId,
    string TableName,
    List<SchemaColumnDto> Columns,
    StagedFileStatusDto Staged
);

public record SchemaColumnDto(
    int OrdinalPosition,
    string ColumnName,
    String ColumnType
);

public record StagedFileStatusDto(
    int Id,
    string Stage,
    string Status,
    string? ErrorCode
);