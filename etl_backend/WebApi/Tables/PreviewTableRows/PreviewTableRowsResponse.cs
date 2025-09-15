namespace WebApi.Tables.PreviewTableRows;

public class PreviewTableRowsResponse
{
    public List<Dictionary<string, object?>> Rows { get; set; } = new();
    public int NextOffset { get; set; }
}