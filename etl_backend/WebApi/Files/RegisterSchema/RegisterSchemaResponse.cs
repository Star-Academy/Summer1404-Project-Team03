namespace WebApi.Files;

public class RegisterSchemaResponse
{
    public int SchemaId { get; set; }
    public string TableName { get; set; } = string.Empty;
    public List<RegisterSchemaColumnResponse> Columns { get; set; } = new();
    public StagedFileStatusResponse Staged { get; set; } = new();
}

public class RegisterSchemaColumnResponse
{
    public int OrdinalPosition { get; set; }
    public string ColumnName { get; set; } = string.Empty;
    public string ColumnType { get; set; } = string.Empty;
}

public class StagedFileStatusResponse
{
    public Guid Id { get; set; }
    public string Stage { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? ErrorCode { get; set; }
}