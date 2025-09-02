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