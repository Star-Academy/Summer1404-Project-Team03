namespace etl_backend.Domain.Entities;

public class DataTableSchema
{
    public int Id { get; set; }
    public string TableName { get; set; } = string.Empty;
    
    public string? OriginalFileName { get; set; }

    public List<DataTableColumn> Columns { get; set; } = new();
}