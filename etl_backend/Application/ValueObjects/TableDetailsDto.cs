namespace Application.ValueObjects;

public sealed class TableDetailsDto
{
    public int SchemaId { get; set; }
    public string TableName { get; set; } = default!;
    public bool PhysicalExists { get; set; }
    public long RowCountApprox { get; set; }
    public long SizeBytes { get; set; }
    public List<ColumnDetailsDto> Columns { get; set; } = new();
}

public sealed class ColumnDetailsDto
{
    public int Ordinal { get; set; }
    public string Name { get; set; } = default!;
    public string Type { get; set; } = default!;
}