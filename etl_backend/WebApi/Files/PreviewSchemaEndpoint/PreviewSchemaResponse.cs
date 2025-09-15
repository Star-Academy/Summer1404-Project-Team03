namespace WebApi.Files;

public class PreviewSchemaResponse
{
    public int StagedFileId { get; set; }
    public List<PreviewSchemaColumnItem> Columns { get; set; } = new();
}

public class PreviewSchemaColumnItem
{
    public int OrdinalPosition { get; set; }
    public string ColumnName { get; set; } = string.Empty;
    public string OriginalColumnName { get; set; } = string.Empty;
    public string ColumnType {get; set; } = string.Empty;
}