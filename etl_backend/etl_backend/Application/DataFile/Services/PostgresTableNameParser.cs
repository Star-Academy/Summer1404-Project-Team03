using etl_backend.Application.DataFile.Abstraction;

namespace etl_backend.Application.DataFile.Services;

public sealed class PostgresTableNameParser : ITableNameParser
{
    public (string Schema, string Table) Parse(string name, string defaultSchema)
    {
        var parts = name.Split('.', 2, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        return parts.Length == 2 ? (parts[0], parts[1]) : (defaultSchema, name);
    }
}