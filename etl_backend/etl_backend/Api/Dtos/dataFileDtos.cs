namespace etl_backend.Api.Dtos;

public sealed class StageFileResponse
{
    public int Id { get; set; }
    public string OriginalFileName { get; set; } = default!;
    public string StoredFilePath { get; set; } = default!;
    public long FileSize { get; set; }
    public DateTime UploadedAt { get; set; }
    public string Stage { get; set; } = default!;
    public string Status { get; set; } = default!;
    public string ErrorCode { get; set; } = default!;
    public string? ErrorMessage { get; set; }
}

public sealed class ColumnPreviewItem
{
    public int OrdinalPosition { get; set; }
    public string ColumnName { get; set; } = default!;
    public string? OriginalColumnName { get; set; }
}

public sealed class ColumnPreviewResponse
{
    public int StagedFileId { get; set; }
    public List<ColumnPreviewItem> Columns { get; set; } = new();
}

public sealed class RegisterSchemaRequest
{
    public List<ColumnTypeSelection> Columns { get; set; } = new();
}

public sealed class ColumnTypeSelection
{
    public int OrdinalPosition { get; set; }
    public string ColumnType { get; set; } = "string";
}

public sealed class ListFilesItem
{
    public int Id { get; set; }
    public string OriginalFileName { get; set; } = default!;
    public string Stage { get; set; } = default!;
    public string Status { get; set; } = default!;
    public int? SchemaId { get; set; }
}
public sealed class StageFileRequest
{
    public IFormFile File { get; set; } = default!;
}
public sealed class TableListItem
{
    public int SchemaId { get; set; }
    public string TableName { get; set; } = default!;
    public string OriginalFileName { get; set; } = "";
    public int ColumnCount { get; set; }
}

public sealed class RenameTableRequest
{
    public string NewTableName { get; set; } = default!;
}

public sealed class TableDetailsDto
{
    public int SchemaId { get; set; }
    public string TableName { get; set; } = default!;
    public bool PhysicalExists { get; set; }
    public long RowCountApprox { get; set; }
    public long SizeBytes { get; set; }
    public List<ColumnDetailsDto> Columns { get; set; } = new();
}

public sealed class ColumnDetailsDto
{
    public int Ordinal { get; set; }
    public string Name { get; set; } = default!;
    public string Type { get; set; } = default!;
}

public sealed class RowPreviewDto
{
    public List<Dictionary<string, object?>> Rows { get; set; } = new();
    public int NextOffset { get; set; }
}

public sealed class RowCountDto
{
    public bool Exact { get; set; }
    public long Count { get; set; }
}
public sealed class StageFilesRequest
{
    public List<IFormFile> Files { get; set; } = new();
}

public sealed class StageFileBatchItem
{
    public string FileName { get; set; } = default!;
    public bool Success { get; set; }
    public StageFileResponse? Data { get; set; }
    public string? Error { get; set; }
}

