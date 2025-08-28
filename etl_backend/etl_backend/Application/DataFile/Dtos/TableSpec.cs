namespace etl_backend.Application.DataFile.Dtos;

public sealed record TableSpec(string Name, IReadOnlyList<ColumnSpec> Columns);