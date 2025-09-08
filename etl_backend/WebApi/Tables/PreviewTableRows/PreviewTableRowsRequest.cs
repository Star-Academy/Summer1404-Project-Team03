namespace WebApi.Tables.PreviewTableRows;

public class PreviewTableRowsRequest
{
    public int SchemaId { get; set; }
    public int Offset { get; set; } = 0;
    public int Limit { get; set; } = 50;
    public string? OrderBy { get; set; }
    public string? Direction { get; set; }
}