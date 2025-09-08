namespace WebApi.Tables.ListColumns;

public class ListColumnsResponse
{
    public List<ColumnListItem> Items { get; set; } = new();
}

public class ColumnListItem
{
    public int Id { get; set; }
    public int OrdinalPosition { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string? OriginalName { get; set; }
}