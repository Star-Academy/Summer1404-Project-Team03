namespace WebApi.Tables.GetTableRowsCount;

public class GetTableRowCountRequest
{
    public int SchemaId { get; set; }
    public bool Exact { get; set; } = false;
}