using etl_backend.Application.DataFile.Abstraction;

namespace etl_backend.Application.DataFile.Services;


public sealed class PostgresTypeMapper : ITypeMapper
{
    public string ToProviderType(string logicalType) => logicalType.ToLowerInvariant() switch
    {
        "string"   => "TEXT",
        // future-proofing:
        "int"      => "INTEGER",
        "long"     => "BIGINT",
        "decimal"  => "NUMERIC",
        "bool"     => "BOOLEAN",
        "datetime" => "TIMESTAMPTZ",
        _          => "TEXT"
    };
}
