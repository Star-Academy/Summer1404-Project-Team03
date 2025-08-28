namespace etl_backend.Application.DataFile.Configurations;

public sealed class PostgresStoreOptions
{
    public string DefaultSchema { get; set; } = "public";
}
