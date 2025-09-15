namespace WebApi.Tables.RenameColumn;

public class RenameColumnRequest
{
    public int SchemaId { get; set; }
    public int ColumnId { get; set; }
    public string NewName { get; set; } = string.Empty;
}