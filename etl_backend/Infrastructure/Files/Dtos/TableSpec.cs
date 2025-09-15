namespace Infrastructure.Files.Dtos;

public sealed record TableSpec(string Name, IReadOnlyList<ColumnSpec> Columns);