namespace WebApi.Files;

public class RegisterSchemaRequest
{
    public int Id { get; set; }
    public List<RegisterSchemaColumnItem> Columns { get; set; } = new();
}

public class RegisterSchemaColumnItem
{
    public int OrdinalPosition { get; set; }
    public string ColumnType { get; set; } = string.Empty;
}