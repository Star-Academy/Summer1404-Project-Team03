using etl_backend.Application.DataFile.Abstraction;
using etl_backend.Application.DataFile.Configurations;

namespace etl_backend.Application.DataFile.Services;
using Microsoft.Extensions.Options;

public sealed class PostgresIdentifierPolicy : IIdentifierPolicy
{
    private readonly string _defaultSchema;
    public PostgresIdentifierPolicy(IOptions<PostgresStoreOptions> opts)
        => _defaultSchema = (opts.Value.DefaultSchema ?? "public").Trim();

    public string DefaultSchema => _defaultSchema;

    public string QuoteIdentifier(string identifier)
    {
        // Double-quote and escape embedded quotes per SQL standard
        var escaped = identifier.Replace("\"", "\"\"");
        return $"\"{escaped}\"";
    }

    public string QualifyTable(string? schema, string table)
    {
        var s = string.IsNullOrWhiteSpace(schema) ? _defaultSchema : schema!;
        return $"{QuoteIdentifier(s)}.{QuoteIdentifier(table)}";
    }
}
