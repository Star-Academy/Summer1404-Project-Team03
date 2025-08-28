namespace etl_backend.Domain.Entities;

public class DataTableColumn
{
    public int Id { get; set; }
    
    public required string ColumnName { get; set; }
    
    public int OrdinalPosition { get; set; }
    
    public string ColumnType { get; set; } = "string"; // default all strings

    public int DataTableSchemaId { get; set; }
    public DataTableSchema? DataTable { get; set; }
}