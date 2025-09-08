namespace WebApi.Tables.GetTableDetails;

public class GetTableDetailsResponse
{
    public int SchemaId { get; set; }
    public string TableName { get; set; } = string.Empty;
    public bool PhysicalExists { get; set; }
    public long RowCountApprox { get; set; }
    public long SizeBytes { get; set; }
    public List<ColumnDetailsDto> Columns { get; set; } = new();
}

public class ColumnDetailsDto
{
    public int Ordinal { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
}