using etl_backend.Application.DataFile.Abstraction;

namespace Infrastructure.Files.PostgresTableServices.HelperServices;


public sealed class PostgresTypeMapper : ITypeMapper
{
    public string ToProviderType(string logicalType) => logicalType.ToLowerInvariant() switch
    {
        "string"   => "TEXT",
        "int"      => "INTEGER",
        "long"     => "BIGINT",
        "decimal"  => "NUMERIC",
        "bool"     => "BOOLEAN",
        "datetime" => "TIMESTAMPTZ",
        _          => "TEXT"
    };
}
